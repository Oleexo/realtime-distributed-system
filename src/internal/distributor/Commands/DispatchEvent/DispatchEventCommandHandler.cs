using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.Common;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchEvent;

public sealed class DispatchEventCommandHandler : BaseDispatch, ICommandHandler<DispatchEventCommand> {
    public DispatchEventCommandHandler(IBrokerPusher             brokerPusher,
                                       IUserConnectionRepository userConnectionRepository)
        : base(brokerPusher, userConnectionRepository) {
    }

    public async Task<Result<Unit>> Handle(DispatchEventCommand request,
                                           CancellationToken    cancellationToken) {
        var @event = new Event {
            Author = request.Author,
            Content = request.Content
        };
        await DispatchToConnectedUsersAsync(request.Recipients, request.Tag, @event, cancellationToken);
        return Unit.Value;
    }
}
