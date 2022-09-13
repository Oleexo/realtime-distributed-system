using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Repositories;

public interface IPusherServerRepository {
    Task<bool> CreateAsync(PusherServer      pusherServer,
                           CancellationToken cancellationToken = default);

    Task<PusherServer?> GetByIdAsync(string            name,
                                     CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PusherServer>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(PusherServer      current,
                           CancellationToken cancellationToken = default);

    Task DeleteAsync(string            id,
                     CancellationToken cancellationToken = default);
}
