namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Api.Models.Requests;

public record RegisterPusherRequest
{
    public string Name { get; init; } = string.Empty;
}