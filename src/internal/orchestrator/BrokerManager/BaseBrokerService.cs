using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager;

public abstract class BaseBrokerService : IBrokerService {
    protected abstract QueueType Type { get; }

    public async Task<QueueInfo> CreateAsync(string            name,
                                             CancellationToken cancellationToken = default) {
        var queueName          = GenerateQueueName(name);
        var queueNameFinalized = await CreateQueue(queueName, cancellationToken);

        return new QueueInfo(Type, queueNameFinalized);
    }

    public abstract Task DestroyAsync(QueueInfo         queueInfo,
                                      CancellationToken cancellationToken = default);

    private string GenerateQueueName(string name) {
        return $"pusher-{name}";
    }

    protected abstract Task<string> CreateQueue(string            queueName,
                                                CancellationToken cancellationToken);
}
