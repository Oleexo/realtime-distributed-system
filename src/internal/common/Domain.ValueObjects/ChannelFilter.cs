namespace Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

public record ChannelFilter {
    public string[] Tags { get; init; } = Array.Empty<string>();
}
