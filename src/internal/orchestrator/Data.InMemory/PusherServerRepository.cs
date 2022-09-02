using System.Collections.Concurrent;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Data.InMemory;

internal class PusherServerRepository : IPusherServerRepository {
    private readonly ConcurrentDictionary<string, PusherServer> _entities;

    public PusherServerRepository() {
        _entities = new ConcurrentDictionary<string, PusherServer>();
    }

    public Task<bool> CreateAsync(PusherServer      pusherServer,
                                  CancellationToken cancellationToken = default) {
        return Task.FromResult(_entities.TryAdd(pusherServer.Id, pusherServer));
    }

    public Task<PusherServer?> GetByIdAsync(string            name,
                                            CancellationToken cancellationToken = default) {
        return _entities.TryGetValue(name, out var server)
                   ? Task.FromResult<PusherServer?>(server)
                   : Task.FromResult<PusherServer?>(null);
    }

    public Task<IReadOnlyCollection<PusherServer>> GetAllAsync(CancellationToken cancellationToken = default) {
        return Task.FromResult<IReadOnlyCollection<PusherServer>>(_entities.Values.ToArray());
    }

    public Task<bool> UpdateAsync(PusherServer      current,
                                  CancellationToken cancellationToken = default) {
        return Task.FromResult(true);
    }
}