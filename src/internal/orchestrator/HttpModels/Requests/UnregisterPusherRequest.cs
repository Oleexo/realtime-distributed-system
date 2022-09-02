namespace Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;

public record UnregisterPusherRequest {
    public string Name { get; init; } = string.Empty;
}
