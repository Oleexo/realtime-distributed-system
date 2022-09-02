using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

public record Message {
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("content")]
    public string Content { get; init; } = string.Empty;
}
