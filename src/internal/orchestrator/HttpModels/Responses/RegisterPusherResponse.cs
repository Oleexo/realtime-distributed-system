using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Responses;

public record RegisterPusherResponse {
    [JsonPropertyName("queue_type")]
    public string QueueType { get; init; }

    [JsonPropertyName("queue_name")]
    public string QueueName { get; init; }
}
