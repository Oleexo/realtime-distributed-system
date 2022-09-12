using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;

public record MessageReadRequest {
    [JsonPropertyName("recipients")]
    public IReadOnlyCollection<string> Recipients { get; init; } = new List<string>();

    [JsonPropertyName("tag")]
    public string Tag { get; init; } = string.Empty;

    [JsonPropertyName("channel_id")]
    public string ChannelId { get; init; } = string.Empty;

    [JsonPropertyName("message_id")]
    public long MessageId { get; init; }

    [JsonPropertyName("user_id")]
    public string UserId { get; init; } = string.Empty;
}
