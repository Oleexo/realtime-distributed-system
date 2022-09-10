using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

public record Message {
    public static Message Empty => new Message();
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("channel_id")]
    public string ChannelId { get; init; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; init; } = string.Empty;

    [JsonPropertyName("parent_id")]
    public long? ParentId { get; init; }
    
    [JsonPropertyName("is_deletion")]
    public bool? IsDeletion { get; init; }
}