using Microsoft.Extensions.Options;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;
using RabbitMQ.Client;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Services;

public class RabbitMqBaseBrokerService : BaseBrokerService {
    private readonly IConnection _client;

    public RabbitMqBaseBrokerService(IOptions<RabbitMqOptions> options) {
        
        var factory = new ConnectionFactory();
// "guest"/"guest" by default, limited to localhost connections
        factory.UserName    = options.Value.UserName;
        factory.Password    = options.Value.Password;
        factory.VirtualHost = options.Value.VirtualHost;
        factory.HostName    = options.Value.Host;

        _client = factory.CreateConnection();
    }

    public override Task DestroyAsync(QueueInfo         queueInfo,
                                      CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }

    protected override QueueType Type => QueueType.RabbitMq;

    protected override Task<string> CreateQueue(string            queueName,
                                                CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}