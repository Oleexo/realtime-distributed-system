using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager.AmazonSqs;

internal class SqsBrokerService : BaseBrokerService {
    private readonly AmazonSQSClient           _client;
    private readonly ILogger<SqsBrokerService> _logger;

    public SqsBrokerService(IOptions<SqsOptions>      options,
                            ILogger<SqsBrokerService> logger) {
        _logger = logger;
        var credentials = new BasicAWSCredentials("Dummy", "Dummy");
        _client = new AmazonSQSClient(credentials, new AmazonSQSConfig {
            ServiceURL = options.Value.Region
        });
    }

    protected override QueueType Type => QueueType.Sqs;

    public override async Task DestroyAsync(QueueInfo         queueInfo,
                                            CancellationToken cancellationToken = default) {
        var request = new DeleteQueueRequest {
            QueueUrl = queueInfo.Name
        };
        var _ = await _client.DeleteQueueAsync(request, cancellationToken);
        _logger.LogDebug("Queue deleted with success: {QueueUrl}", queueInfo.Name);
    }

    protected override async Task<string> CreateQueue(string            queueName,
                                                      CancellationToken cancellationToken) {
        var request = new CreateQueueRequest {
            QueueName = queueName
        };
        var response = await _client.CreateQueueAsync(request, cancellationToken);
        _logger.LogDebug("Queue created with success: {QueueUrl}", response.QueueUrl);
        return response.QueueUrl;
    }
}
