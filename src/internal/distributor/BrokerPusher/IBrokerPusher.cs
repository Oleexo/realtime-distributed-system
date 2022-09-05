using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;

public interface IBrokerPusher {
    public QueueType Type { get; }
    public Task PushMessageAsync(MessageWrapper    message,
                                 QueueInfo         queue,
                                 CancellationToken cancellationToken = default);
}