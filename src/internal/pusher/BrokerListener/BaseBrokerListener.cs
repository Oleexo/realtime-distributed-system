using System.Text.Json;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener;

public abstract class BaseBrokerListener : IBrokerListener {
    protected abstract QueueType Type { get; }

    public void Listen(QueueType                  queueType,
                       string                     queueName,
                       Func<MessageWrapper, Task> messageHandler) {
        if (queueType != Type) {
            throw new InvalidOperationException("Invalid queue type");
        }

        StartListen(queueName, messageHandler);
    }

    public abstract Task StopAsync();

    protected abstract void StartListen(string                     queueName,
                                        Func<MessageWrapper, Task> messageHandler);

    protected MessageWrapper? DeserializeMessage(string payload) {
        return JsonSerializer.Deserialize<MessageWrapper>(payload);
    }
}
