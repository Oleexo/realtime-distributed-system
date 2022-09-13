using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;

public record UnregisterPusherRequest {
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}
