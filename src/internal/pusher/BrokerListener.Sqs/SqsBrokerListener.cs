using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener.AmazonSqs;

public sealed class SqsBrokerListener : BaseBrokerListener, IDisposable {
    private readonly AmazonSQSClient            _client;
    private readonly ILogger<SqsBrokerListener> _logger;
    private          bool                       _isStopping;
    private          Task?                      _pollingTask;
    private          CancellationTokenSource?   _stopping;

    public SqsBrokerListener(IOptions<SqsOptions>       options,
                             ILogger<SqsBrokerListener> logger) {
        _logger = logger;
        var credentials = new BasicAWSCredentials("Dummy", "Dummy");
        _client = new AmazonSQSClient(credentials, new AmazonSQSConfig {
            ServiceURL = options.Value.Region
        });
        _isStopping = false;
    }

    protected override QueueType Type => QueueType.Sqs;

    public void Dispose() {
        _pollingTask?.Dispose();
        _client.Dispose();
    }

    public override async Task StopAsync() {
        if (_isStopping == false  &&
            _stopping is not null &&
            _pollingTask is not null) {
            _isStopping = true;
            _stopping.Cancel();
            await _pollingTask;
        }
    }

    protected override void StartListen(string             queueName,
                                        Func<Letter, Task> messageHandler) {
        _stopping = new CancellationTokenSource();
        _pollingTask = Task.Run(async () => {
            try {
                while (!_isStopping) {
                    var queueUrl = queueName.Replace("localstack", "localhost");
                    var request = new ReceiveMessageRequest {
                        QueueUrl            = queueUrl,
                        VisibilityTimeout   = 30,
                        WaitTimeSeconds     = 3,
                        MaxNumberOfMessages = 10
                    };
                    var response = await _client.ReceiveMessageAsync(request, _stopping.Token);
                    _logger.LogDebug("Messages received {Count}", response.Messages.Count);
                    foreach (var message in response.Messages) {
                        var wrapper = DeserializeMessage(message.Body);
                        if (wrapper is not null) {
                            await messageHandler(wrapper);
                        }

                        await _client.DeleteMessageAsync(queueName, message.ReceiptHandle);
                    }
                }
            }
            catch (OperationCanceledException exception) when (exception.CancellationToken == _stopping.Token) {
            }
            catch (Exception e) {
                _logger.LogError(e, "Listen Sqs message failed");
                throw;
            }
        });
    }
}
