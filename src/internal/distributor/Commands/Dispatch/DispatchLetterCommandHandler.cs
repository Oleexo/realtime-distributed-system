using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.Common;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.Dispatch;

public sealed class DispatchLetterCommandHandler : BaseDispatch, ICommandHandler<DispatchLetterCommand> {
    public DispatchLetterCommandHandler(IBrokerPusher             brokerPusher,
                                        IUserConnectionRepository userConnectionRepository)
        : base(brokerPusher, userConnectionRepository) {
    }

    public Task<Result<Unit>> Handle(DispatchLetterCommand request,
                                     CancellationToken     cancellationToken) {
        return DispatchToConnectedUsersAsync(request.Letter, request.Letter.Recipients, cancellationToken);
    }
}
