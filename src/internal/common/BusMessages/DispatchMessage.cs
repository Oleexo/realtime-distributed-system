using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Common.BusMessages;

public record DispatchMessage {
    public Message                     Message    { get; init; } = Message.Empty;
    public string                      Tag        { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Recipients { get; init; } = Array.Empty<string>();
}
