using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Store.Domain.Events;

public record MessageCreated : INotification {
    public Message                     Message    { get; init; } = Message.Empty;
    public string                      Tag        { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Recipients { get; init; } = Array.Empty<string>();
}