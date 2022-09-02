using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.PusherRefresh;

public record RefreshPusherCommand : ICommand {
    public string Name { get; init; } = string.Empty;
}
