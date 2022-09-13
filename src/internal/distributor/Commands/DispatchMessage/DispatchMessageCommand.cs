using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchMessage;

public record DispatchMessageCommand : ICommand {
    public Message                     Message    { get; init; } = Message.Empty;
    public IReadOnlyCollection<string> Recipients { get; init; } = new List<string>();
    public string                      Tag        { get; init; } = string.Empty;
}
