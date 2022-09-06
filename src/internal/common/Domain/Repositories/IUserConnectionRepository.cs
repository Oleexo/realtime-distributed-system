using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;

public interface IUserConnectionRepository {
    Task<bool> CreateAsync(UserConnection    userConnection,
                           CancellationToken cancellationToken = default);

    Task DeleteAsync(string            id,
                     CancellationToken cancellationToken = default);

    Task UpdateAsync(UserConnection    userConnection,
                     CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<UserConnection>> GetConnectedUsersWithTag(string            tag,
                                                                       CancellationToken cancellationToken = default);
}