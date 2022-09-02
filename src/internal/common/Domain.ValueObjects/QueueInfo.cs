namespace Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

public record QueueInfo {
    public static QueueInfo Empty = new(QueueType.Unknown, string.Empty);

    public QueueInfo(QueueType type,
                     string    name) {
        Name = name;
        Type = type;
    }

    public string    Name { get; }
    public QueueType Type { get; }
}
