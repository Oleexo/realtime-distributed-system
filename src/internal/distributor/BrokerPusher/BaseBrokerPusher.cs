using System.Text.Json;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;

public abstract class BaseBrokerPusher : IBrokerPusher {
    public Task PushAsync(Letter            letter,
                          QueueInfo         queue,
                          CancellationToken cancellationToken = default) {
        if (!IsSupported(queue.Type)) {
            throw new InvalidOperationException("The queue is not the supported by the current pusher");
        }

        var json = JsonSerializer.Serialize(letter);
        return SendMessageAsync(json, queue.Name, cancellationToken);
    }

    protected abstract bool IsSupported(QueueType queueType);

    protected abstract Task SendMessageAsync(string            content,
                                             string            queueName,
                                             CancellationToken cancellationToken = default);
}
