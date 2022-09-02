using MediatR;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.PusherRefresh;

public sealed class RefreshPusherCommandHandler : ICommandHandler<RefreshPusherCommand> {
    private readonly IPusherServerRepository              _pusherServerRepository;
    private readonly ILogger<RefreshPusherCommandHandler> _logger;

    public RefreshPusherCommandHandler(IPusherServerRepository              pusherServerRepository,
                                       ILogger<RefreshPusherCommandHandler> logger) {
        _pusherServerRepository = pusherServerRepository;
        _logger                 = logger;
    }

    public async Task<Result<Unit>> Handle(RefreshPusherCommand request,
                                           CancellationToken    cancellationToken) {
        var current = await _pusherServerRepository.GetByIdAsync(request.Name, cancellationToken);
        if (current == null) {
            _logger.LogWarning("Missing pusher server {PusherName}", request.Name);
            return Unit.Value;
        }
        
        current.LastPing = DateTime.UtcNow;
        await _pusherServerRepository.UpdateAsync(current, cancellationToken);
        return Unit.Value;
    }
}
