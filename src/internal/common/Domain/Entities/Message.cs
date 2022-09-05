using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

public record Message {
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("channel_id")]
    public string ChannelId { get; init; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; init; } = string.Empty;
}

public record Event {
    [JsonPropertyName("content")]
    public string Content { get; init; } = string.Empty;
}
