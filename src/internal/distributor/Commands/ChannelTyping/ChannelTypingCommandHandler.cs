using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.Common;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.ChannelTyping;

public sealed class ChannelTypingCommandHandler : BaseDispatch, ICommandHandler<ChannelTypingCommand> {
    public ChannelTypingCommandHandler(IBrokerPusher             brokerPusher,
                                       IUserConnectionRepository userConnectionRepository)
        : base(brokerPusher, userConnectionRepository) {
    }

    public Task<Result<Unit>> Handle(ChannelTypingCommand request,
                                     CancellationToken    cancellationToken) {
        return DispatchToConnectedUsersAsync(request.Recipients, request.Tag, new Event {
            Author  = request.UserId,
            Content = request.IsTyping ? "IsTyping" : "StopTyping"
        }, cancellationToken);
    }
}
