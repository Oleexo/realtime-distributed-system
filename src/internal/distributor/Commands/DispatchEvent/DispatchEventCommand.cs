using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchEvent;

public record DispatchEventCommand : ICommand {
    public string                      Content    { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Recipients { get; init; } = new List<string>();
    public string                      Tag        { get; init; } = string.Empty;
}
