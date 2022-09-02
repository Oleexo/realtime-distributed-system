using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

public record MessageWrapper {
    [JsonPropertyName("message")]
    public Message Message { get; init; } = new();

    [JsonPropertyName("recipients")]
    public IReadOnlyCollection<string> Recipients { get; init; } = Array.Empty<string>();

    [JsonPropertyName("tags")]
    public IReadOnlyCollection<string> Tags { get; init; } = Array.Empty<string>();
}
