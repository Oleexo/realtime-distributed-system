using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.Common;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchMessage;

public sealed class DispatchMessageCommandHandler : BaseDispatch, ICommandHandler<DispatchMessageCommand> {
    private readonly IMessageRepository _messageRepository;

    public DispatchMessageCommandHandler(IBrokerPusher             brokerPusher,
                                         IUserConnectionRepository userConnectionRepository,
                                         IMessageRepository        messageRepository)
        : base(brokerPusher, userConnectionRepository) {
        _messageRepository = messageRepository;
    }

    public async Task<Result<Unit>> Handle(DispatchMessageCommand request,
                                           CancellationToken      cancellationToken) {
        await DispatchToConnectedUsersAsync(request.Recipients, request.Tag, request.Message, cancellationToken);
        return Unit.Value;
    }
}
