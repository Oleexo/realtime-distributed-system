using MediatR;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.CleanDeadServer;

public sealed class CleanDeadServerCommandHandler : ICommandHandler<CleanDeadServerCommand> {
    private readonly IBrokerService                         _brokerService;
    private readonly ILogger<CleanDeadServerCommandHandler> _logger;
    private readonly IPusherServerRepository                _pusherServerRepository;

    public CleanDeadServerCommandHandler(IPusherServerRepository                pusherServerRepository,
                                         IBrokerService                         brokerService,
                                         ILogger<CleanDeadServerCommandHandler> logger) {
        _pusherServerRepository = pusherServerRepository;
        _brokerService          = brokerService;
        _logger                 = logger;
    }

    public async Task<Result<Unit>> Handle(CleanDeadServerCommand request,
                                           CancellationToken      cancellationToken) {
        var servers = await _pusherServerRepository.GetAllAsync(cancellationToken);

        foreach (var server in servers) {
            if (server.LastPing.AddMinutes(1) <= DateTime.Now) {
                continue;
            }

            _logger.LogInformation("Server {ServerName} is dead since {LastPing}", server.Id, server.LastPing);
            await _brokerService.DestroyAsync(server.Queue, cancellationToken);
        }

        return Unit.Value;
    }
}
