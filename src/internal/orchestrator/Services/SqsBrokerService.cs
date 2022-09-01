using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Services;

internal class SqsBrokerService : BaseBrokerService {
    private readonly AmazonSQSClient _client;

    public SqsBrokerService(IOptions<SqsOptions> options) {
        var credentials = new BasicAWSCredentials("Dummy", "Dummy");
        _client = new AmazonSQSClient(credentials, RegionEndpoint.EUCentral1);
    }

    public override Task DestroyAsync(QueueInfo         queueInfo,
                                            CancellationToken cancellationToken = default) {

        var request = new DeleteQueueRequest {
            QueueUrl = queueInfo.Name
        };
        return _client.DeleteQueueAsync(request, cancellationToken);
    }

    protected override QueueType Type => QueueType.Sqs;

    protected override async Task<string> CreateQueue(string            queueName,
                                                      CancellationToken cancellationToken) {
        var request = new CreateQueueRequest {
            QueueName = queueName
        };
        var response = await _client.CreateQueueAsync(request, cancellationToken);
        return response.QueueUrl;
    }
}