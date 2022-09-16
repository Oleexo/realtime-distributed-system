using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Requests;

public record ChannelTypingRequest {
    [JsonPropertyName("user_id")]
    public string UserId { get; init; } = string.Empty;

    [JsonPropertyName("channel_id")]
    public string ChannelId { get; init; } = string.Empty;

    [JsonPropertyName("recipients")]
    public IReadOnlyCollection<string> Recipients { get; init; } = Array.Empty<string>();

    [JsonPropertyName("tag")]
    public string Tag { get; init; } = string.Empty;

    [JsonPropertyName("is_typing")]
    public bool IsTyping { get; init; }
}
