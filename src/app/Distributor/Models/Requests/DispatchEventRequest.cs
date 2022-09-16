using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Requests;

public record DispatchEventRequest {
    [JsonPropertyName("content")]
    public string Content { get; init; } = string.Empty;

    [JsonPropertyName("recipients")]
    public IReadOnlyCollection<string> Recipients { get; init; } = new List<string>();

    [JsonPropertyName("tag")]
    public string Tag { get; init; } = string.Empty;

    [JsonPropertyName("author")]
    public string Author { get; init; } = string.Empty;
}