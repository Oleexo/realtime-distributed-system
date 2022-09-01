namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Services;

public record RabbitMqOptions {
    public string Host        { get; set; }
    public string UserName    { get; set; }
    public string Password    { get; set; }
    public string VirtualHost { get; set; }
}