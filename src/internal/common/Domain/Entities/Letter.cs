using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

public record Letter {
    [JsonPropertyName("message")]
    public Message? Message { get; init; }

    [JsonPropertyName("event")]
    public Event? Event { get; init; }

    [JsonPropertyName("type")]
    public LetterType Type { get; init; }

    [JsonPropertyName("recipients")]
    public IReadOnlyCollection<string> Recipients { get; init; } = Array.Empty<string>();

    [JsonPropertyName("tag")]
    public string Tag { get; init; } = string.Empty;
}
