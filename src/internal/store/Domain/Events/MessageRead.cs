using MediatR;

namespace Oleexo.RealtimeDistributedSystem.Store.Domain.Events;

public record MessageRead : INotification {
    public string                      ChannelId  { get; init; } = string.Empty;
    public string                      Tag        { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Recipients { get; init; } = Array.Empty<string>();
    public long                        MessageId  { get; init; }
    public string                      UserId     { get; init; } = string.Empty;
}
