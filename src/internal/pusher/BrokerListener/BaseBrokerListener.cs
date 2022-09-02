namespace Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener;

public abstract class BaseBrokerListener : IBrokerListener {
    protected BaseBrokerListener() {
    }

    public void Listen(string             queueType,
                       string             queueName,
                       Func<string, Task> messageHandler) {
        if (queueType != Type) {
            throw new InvalidOperationException("Invalid queue type");
        }

        StartListen(queueName, messageHandler);
    }

    public abstract Task StopAsync();
    protected abstract string Type { get; }

    protected abstract void StartListen(string             queueName,
                                        Func<string, Task> messageHandler);
    
}
