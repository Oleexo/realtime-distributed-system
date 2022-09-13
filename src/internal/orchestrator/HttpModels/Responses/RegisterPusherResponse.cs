using System.Text.Json.Serialization;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Responses;

public record RegisterPusherResponse {
    [JsonPropertyName("queue_type")]
    public QueueType QueueType { get; init; }

    [JsonPropertyName("queue_name")]
    public string QueueName { get; init; } = string.Empty;
}
