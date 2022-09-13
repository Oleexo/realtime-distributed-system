using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;

public class PusherServer : PusherServer.IData {
    public PusherServer(string    name,
                        QueueInfo queueInfo) {
        Id        = name;
        Queue     = queueInfo;
        CreatedAt = DateTime.UtcNow;
        LastSeen  = CreatedAt;
    }

    public PusherServer(IData data) {
        Id        = data.Id;
        Queue     = data.Queue;
        CreatedAt = data.CreatedAt;
        LastSeen  = data.LastSeen;
    }

    public string         Id        { get; }
    public QueueInfo      Queue     { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset LastSeen  { get; set; }

    public interface IData {
        public string         Id        { get; }
        public QueueInfo      Queue     { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset LastSeen  { get; }
    }
}
