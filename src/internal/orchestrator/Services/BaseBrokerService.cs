using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Services;

public abstract class BaseBrokerService : IBrokerService {
    public async Task<QueueInfo> CreateAsync(string            name,
                                             CancellationToken cancellationToken = default) {
        var queueName          = GenerateQueueName(name);
        var queueNameFinalized = await CreateQueue(queueName, cancellationToken);

        return new QueueInfo(Type, queueNameFinalized);
    }

    public abstract Task DestroyAsync(QueueInfo         queueInfo,
                                      CancellationToken cancellationToken = default);

    protected abstract QueueType Type { get; }

    private string GenerateQueueName(string name) {
        return $"pusher-{name}";
    }

    protected abstract Task<string> CreateQueue(string            queueName,
                                                CancellationToken cancellationToken);
}
