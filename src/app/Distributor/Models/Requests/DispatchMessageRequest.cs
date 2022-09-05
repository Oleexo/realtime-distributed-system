using System.Text.Json.Serialization;

namespace Distributor.Models.Requests;

public record DispatchMessageRequest {
    [JsonPropertyName("content")]
    public string                      Content    { get; init; } = string.Empty;
    [JsonPropertyName("recipients")]
    public IReadOnlyCollection<string> Recipients { get; init; } = new List<string>();
    [JsonPropertyName("tag")]
    public string                      Tag        { get; init; } = string.Empty;
    [JsonPropertyName("channel_id")]
    public string                      ChannelId  { get; init; } = string.Empty;
}
