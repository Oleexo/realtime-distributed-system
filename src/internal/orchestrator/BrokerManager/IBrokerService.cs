using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager;

public interface IBrokerService {
    Task<QueueInfo> CreateAsync(string            name,
                                CancellationToken cancellationToken = default);

    Task DestroyAsync(QueueInfo         queueInfo,
                      CancellationToken cancellationToken = default);
}
