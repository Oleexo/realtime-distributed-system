using Oleexo.RealtimeDistributedSystem.Common.Queries;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Queries.RetrieveMultipleServer;

public record RetrieveMultipleServerQuery : IQuery<IReadOnlyCollection<PusherServer>> {
    
}