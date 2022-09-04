using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;
using Oleexo.RealtimeDistributedSystem.DomainDrivenDesign;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;


public class PusherServer : BaseAggregateRoot<string>, PusherServer.IData {
    public PusherServer(string    name,
                        QueueInfo queueInfo)
        : base(name) {
        Queue     = queueInfo;
        CreatedAt = DateTime.UtcNow;
        LastSeen  = CreatedAt;
    }

    public PusherServer(IData data) :base(data.Id) {
        Queue     = data.Queue;
        CreatedAt = data.CreatedAt;
        LastSeen  = data.LastSeen;
    }

    public QueueInfo      Queue     { get;  }
    public DateTimeOffset CreatedAt { get;  }
    public DateTimeOffset LastSeen  { get; set; }
    
    public interface IData {
        public string    Id        { get; }
        public QueueInfo Queue     { get; }
        public DateTimeOffset  CreatedAt { get; }
        public DateTimeOffset  LastSeen  { get; }
    }
}
