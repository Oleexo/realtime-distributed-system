using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

public record UserConnection {
    public string         Id          => $"{UserId}:{DeviceId}";
    public string         UserId      { get; init; } = string.Empty;
    public string         DeviceId    { get; init; } = string.Empty;
    public ChannelFilter  Filter      { get; init; } = new();
    public DateTimeOffset ConnectedAt { get; init; }
    public DateTimeOffset LastSeen    { get; init; }
    public QueueInfo      Queue       { get; init; } = QueueInfo.Empty;
}
