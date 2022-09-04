using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.RegisterPusher;

public sealed class RegisterPusherCommandHandler : ICommandHandler<RegisterPusherCommand, RegisterPusherResult> {
    private readonly IBrokerService                        _brokerService;
    private readonly ILogger<RegisterPusherCommandHandler> _logger;
    private readonly IPusherServerRepository               _pusherServerRepository;

    public RegisterPusherCommandHandler(IPusherServerRepository               pusherServerRepository,
                                        IBrokerService                        brokerService,
                                        ILogger<RegisterPusherCommandHandler> logger) {
        _pusherServerRepository = pusherServerRepository;
        _brokerService          = brokerService;
        _logger                 = logger;
    }

    public async Task<Result<RegisterPusherResult>> Handle(RegisterPusherCommand command,
                                                           CancellationToken     cancellationToken) {
        var queueInfo = await CreateBroker(command.Name, cancellationToken);
        if (queueInfo.IsFaulted) {
            return Result<RegisterPusherResult>.Fail(queueInfo.Error);
        }

        var pusherServer = new PusherServer(command.Name, queueInfo.Value);
        var isSaved      = await _pusherServerRepository.CreateAsync(pusherServer, cancellationToken);
        if (!isSaved) {
            await DestroyBroker(pusherServer.Queue);
        }

        _logger.LogInformation("Pusher server register {PusherName} at {RegisterDate}", 
                               pusherServer.Id,
                               pusherServer.CreatedAt);
        return new RegisterPusherResult(pusherServer.Queue.Type, pusherServer.Queue.Name);
    }

    private async Task<Result<QueueInfo>> CreateBroker(string            name,
                                                       CancellationToken cancellationToken) {
        return await _brokerService.CreateAsync(name, cancellationToken);
    }

    private async Task DestroyBroker(QueueInfo queueInfo) {
        await _brokerService.DestroyAsync(queueInfo);
    }
}
