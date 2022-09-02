using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;

public record RegisterPusherRequest
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}