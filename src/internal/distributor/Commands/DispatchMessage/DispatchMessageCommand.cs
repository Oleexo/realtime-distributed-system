using MediatR.Pipeline;
using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchMessage;

public record DispatchMessageCommand : ICommand<long> {
    public string                      ChannelId  { get; init; } = string.Empty;
    public string                      Content    { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Recipients { get; init; } = new List<string>();
    public string                      Tag        { get; init; } = string.Empty;
}
