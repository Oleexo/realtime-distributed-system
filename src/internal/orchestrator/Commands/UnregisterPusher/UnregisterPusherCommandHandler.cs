using MediatR;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.UnregisterPusher;

public sealed class UnregisterPusherCommandHandler : ICommandHandler<UnregisterPusherCommand> {
    private readonly IBrokerService                          _brokerService;
    private readonly ILogger<UnregisterPusherCommandHandler> _logger;
    private readonly IPusherServerRepository                 _pusherServerRepository;

    public UnregisterPusherCommandHandler(IBrokerService                          brokerService,
                                          IPusherServerRepository                 pusherServerRepository,
                                          ILogger<UnregisterPusherCommandHandler> logger) {
        _brokerService          = brokerService;
        _pusherServerRepository = pusherServerRepository;
        _logger                 = logger;
    }

    public async Task<Result<Unit>> Handle(UnregisterPusherCommand request,
                                           CancellationToken       cancellationToken) {
        var current = await _pusherServerRepository.GetByIdAsync(request.Name, cancellationToken);
        if (current == null) {
            _logger.LogInformation("The pusher {PusherName} doesn't exists", request.Name);
            return Unit.Value;
        }

        await _brokerService.DestroyAsync(current.Queue, cancellationToken);
        return Unit.Value;
    }
}
