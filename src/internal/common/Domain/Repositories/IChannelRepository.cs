using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;

public interface IChannelRepository {
    Task<Channel?> GetByIdAsync(string            id,
                                CancellationToken cancellationToken = default);

    Task StoreAsync(Channel           channel,
                    CancellationToken cancellationToken = default);
}
