namespace Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager.AmazonSqs;

public record SqsOptions {
    public string Region { get; set; } = string.Empty;
}