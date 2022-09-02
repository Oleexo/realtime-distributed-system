using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener.AmazonSqs; 

public sealed class SqsBrokerListener : BaseBrokerListener, IDisposable {
    private readonly ILogger<SqsBrokerListener> _logger;
    private          CancellationTokenSource?   _stopping;
    private          Task?                      _pollingTask;
    private          bool                       _isStopping;
    private readonly AmazonSQSClient            _client;

    public SqsBrokerListener(IOptions<SqsOptions> options, ILogger<SqsBrokerListener> logger) {
        _logger = logger;
        var credentials = new BasicAWSCredentials("Dummy", "Dummy");
        _client = new AmazonSQSClient(credentials, new AmazonSQSConfig {
            ServiceURL = options.Value.Region
        });
        _isStopping = false;
    }

    public override async Task StopAsync() {
        if (_isStopping == false && 
            _stopping is not null &&
            _pollingTask is not null) {
            _isStopping = true;
            _stopping.Cancel();
            await _pollingTask;
        }
    }

    protected override string Type => "Sqs";

    protected override void StartListen(string queueName,
                                        Func<string, Task> messageHandler) {
        _stopping = new CancellationTokenSource();
        _pollingTask = Task.Run(async () => {
            try {
                while (!_isStopping) {
                    var request = new ReceiveMessageRequest {
                        QueueUrl          = queueName,
                        VisibilityTimeout = 30,
                        WaitTimeSeconds   = 3
                    };
                    var response = await _client.ReceiveMessageAsync(request, _stopping.Token);
                    foreach (var message in response.Messages) {
                        await messageHandler(message.Body);
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

    public void Dispose() {
        _pollingTask?.Dispose();
        _client.Dispose();
    }
}
