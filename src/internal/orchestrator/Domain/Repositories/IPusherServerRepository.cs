using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Repositories; 

public interface IPusherServerRepository {
    Task<bool> CreateAsync(PusherServer      pusherServer,
                                           CancellationToken cancellationToken = default);
}