namespace Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener;

public interface IBrokerListener  {
    void Listen(string             queueType,
                string             queueName,
                Func<string, Task> messageHandler);

    Task StopAsync();
}
