using Oleexo.RealtimeDistributedSystem.DomainDrivenDesign;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;

public class PusherServer : BaseAggregateRoot<string> {
    public PusherServer(string name, QueueInfo queueInfo)
        : base(name) {
        Queue     = queueInfo;
        CreatedAt = DateTime.UtcNow;
        LastPing  = CreatedAt;
    }

    public QueueInfo Queue     { get; private set; }
    public DateTime  CreatedAt { get; private set; }
    public DateTime  LastPing  { get; private set; }
}

public enum QueueType {
    Sqs,
    RabbitMq
}

public record QueueInfo {
    public QueueInfo(QueueType type,
                     string    name) {
        Name = name;
        Type = type;
    }

    public string    Name { get; }
    public QueueType Type { get; }
}
