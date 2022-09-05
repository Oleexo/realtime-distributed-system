using System.Text.Json;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher.Sqs;

internal sealed class SqsBrokerPusher : BaseBrokerPusher {
    private readonly ILogger<SqsBrokerPusher> _logger;
    private readonly AmazonSQSClient          _client;

    public SqsBrokerPusher(IOptions<SqsOptions>     options,
                           ILogger<SqsBrokerPusher> logger) {
        _logger = logger;
        var credentials = new BasicAWSCredentials("Dummy", "Dummy");
        _client = new AmazonSQSClient(credentials, new AmazonSQSConfig {
            ServiceURL = options.Value.Region
        });
    }

    public override QueueType Type => QueueType.Sqs;

    protected override Task SendMessageAsync(string            content,
                                             string            queueName,
                                             CancellationToken cancellationToken = default) {
        var request = new SendMessageRequest {
            MessageBody = content,
            QueueUrl    = queueName
        };
        return _client.SendMessageAsync(request, cancellationToken);
    }
}
