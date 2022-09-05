using System.Text.Json;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;

public abstract class BaseBrokerPusher : IBrokerPusher {
    public abstract QueueType Type { get; }

    public Task PushMessageAsync(MessageWrapper    message,
                                 QueueInfo         queue,
                                 CancellationToken cancellationToken = default) {
        if (queue.Type != Type) {
            throw new InvalidOperationException("The queue is not the supported by the current pusher");
        }
        var json = JsonSerializer.Serialize(message);
        return SendMessageAsync(json, queue.Name, cancellationToken);
    }

    protected abstract Task SendMessageAsync(string            content,
                                             string            queueName,
                                             CancellationToken cancellationToken = default);
}
