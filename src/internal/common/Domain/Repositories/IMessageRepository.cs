using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;

public interface IMessageRepository {
    Task CreateAsync(Message           message,
                     CancellationToken cancellationToken = default);

    Task<Message?> GetByIdAsync(string channelId,
                                long   messageId);
}
