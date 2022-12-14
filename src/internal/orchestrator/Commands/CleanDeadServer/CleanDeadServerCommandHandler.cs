using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.CleanDeadServer;

public sealed class CleanDeadServerCommandHandler : ICommandHandler<CleanDeadServerCommand> {
    private readonly IBrokerService                         _brokerService;
    private readonly ServiceOptions                         _configuration;
    private readonly ILogger<CleanDeadServerCommandHandler> _logger;
    private readonly IPusherServerRepository                _pusherServerRepository;

    public CleanDeadServerCommandHandler(IPusherServerRepository                pusherServerRepository,
                                         IBrokerService                         brokerService,
                                         IOptions<ServiceOptions>               options,
                                         ILogger<CleanDeadServerCommandHandler> logger) {
        _pusherServerRepository = pusherServerRepository;
        _brokerService          = brokerService;
        _logger                 = logger;
        _configuration          = options.Value;
    }

    public async Task<Result<Unit>> Handle(CleanDeadServerCommand request,
                                           CancellationToken      cancellationToken) {
        var servers = await _pusherServerRepository.GetAllAsync(cancellationToken);

        var removedServers = 0;
        foreach (var server in servers) {
            if (server.LastSeen.AddSeconds(_configuration.PusherDeadAfterInterval) >= DateTimeOffset.UtcNow) {
                continue;
            }

            var since = DateTimeOffset.UtcNow - server.LastSeen;
            _logger.LogInformation("Server {ServerName} is dead since {LastPing} - {Duration}",
                                   server.Id,
                                   server.LastSeen,
                                   since);
            await _brokerService.DestroyAsync(server.Queue, cancellationToken);
            await _pusherServerRepository.DeleteAsync(server.Id, cancellationToken);
            removedServers += 1;
        }

        _logger.LogInformation("{AliveServer} servers alive. {DeadServers} servers dead",
                               servers.Count - removedServers,
                               removedServers);
        return Unit.Value;
    }
}
