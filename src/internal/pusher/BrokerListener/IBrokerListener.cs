using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener;

public interface IBrokerListener {
    void Listen(QueueType          queueType,
                string             queueName,
                Func<Letter, Task> messageHandler);

    Task StopAsync();
}
