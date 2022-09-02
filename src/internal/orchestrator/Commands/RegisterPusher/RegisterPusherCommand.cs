using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.RegisterPusher;

public record RegisterPusherCommand : ICommand<RegisterPusherResult> {
    public string Name { get; init; } = string.Empty;
}