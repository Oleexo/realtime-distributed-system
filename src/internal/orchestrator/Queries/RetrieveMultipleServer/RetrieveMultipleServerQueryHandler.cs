using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Common.Queries;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Queries.RetrieveMultipleServer;

public sealed class RetrieveMultipleServerQueryHandler : IQueryHandler<RetrieveMultipleServerQuery, IReadOnlyCollection<PusherServer>> {
    private readonly IPusherServerRepository _pusherServerRepository;

    public RetrieveMultipleServerQueryHandler(IPusherServerRepository pusherServerRepository) {
        _pusherServerRepository = pusherServerRepository;
    }

    public async Task<Result<IReadOnlyCollection<PusherServer>>> Handle(RetrieveMultipleServerQuery request,
                                                                        CancellationToken           cancellationToken) {
        var servers = await _pusherServerRepository.GetAllAsync(cancellationToken);
        return Result<IReadOnlyCollection<PusherServer>>.Success(servers);
    }
}
