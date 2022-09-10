using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Responses;

public record PusherServerResponse {
    public string         Id        { get; init; } = string.Empty;
    public QueueInfo      Queue     { get; init; } = QueueInfo.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset LastSeen  { get; init; }
}
