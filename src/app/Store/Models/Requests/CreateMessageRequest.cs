using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;

public record CreateMessageRequest {
    [JsonPropertyName("content")]
    public string Content { get; init; } = string.Empty;

    [JsonPropertyName("author")]
    public string Author { get; init; } = string.Empty;

    [JsonPropertyName("recipients")]
    public IReadOnlyCollection<string> Recipients { get; init; } = new List<string>();

    [JsonPropertyName("tag")]
    public string Tag { get; init; } = string.Empty;

    [JsonPropertyName("channel_id")]
    public string ChannelId { get; init; } = string.Empty;
}
