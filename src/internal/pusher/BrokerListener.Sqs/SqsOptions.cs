namespace Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener.AmazonSqs;

public record SqsOptions {
    public string Region { get; set; } = string.Empty;
}
