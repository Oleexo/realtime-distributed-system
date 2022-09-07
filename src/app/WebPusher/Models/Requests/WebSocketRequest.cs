using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.WebPusher.Api.Models.Requests;

public record WebSocketRequest {
    [JsonPropertyName("user_id")]
    public string UserId { get; init; } = string.Empty;

    [JsonPropertyName("device_id")]
    public string DeviceId { get; init; } = string.Empty;

    [JsonPropertyName("tags")]
    public IReadOnlyCollection<string> Tags { get; init; } = Array.Empty<string>();
}
