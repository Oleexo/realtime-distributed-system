using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

public record Event {
    [JsonPropertyName("content")]
    public string Content { get; init; } = string.Empty;
}
