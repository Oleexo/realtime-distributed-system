using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;
using Oleexo.RealtimeDistributedSystem.DomainDrivenDesign;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;

public class PusherServer : BaseAggregateRoot<string> {
    public PusherServer(string    name,
                        QueueInfo queueInfo)
        : base(name) {
        Queue     = queueInfo;
        CreatedAt = DateTime.UtcNow;
        LastSeen  = CreatedAt;
    }

    public QueueInfo Queue     { get; }
    public DateTime  CreatedAt { get; }
    public DateTime  LastSeen  { get; set; }
}
