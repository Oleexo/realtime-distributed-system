using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.UnregisterPusher;

public record UnregisterPusherCommand : ICommand {
    public string Name { get; init; } = string.Empty;
}
