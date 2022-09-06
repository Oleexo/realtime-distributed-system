using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.Common;
using Oleexo.RealtimeDistributedSystem.Distributor.SnowflakeGen;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchMessage;

public sealed class DispatchMessageCommandHandler : BaseDispatch, ICommandHandler<DispatchMessageCommand, long> {
    private readonly ISnowflakeGen      _snowflakeGen;
    private readonly IMessageRepository _messageRepository;

    public DispatchMessageCommandHandler(IBrokerPusher             brokerPusher,
                                         ISnowflakeGen             snowflakeGen,
                                         IUserConnectionRepository userConnectionRepository,
                                         IMessageRepository        messageRepository)
        : base(brokerPusher, userConnectionRepository) {
        _snowflakeGen      = snowflakeGen;
        _messageRepository = messageRepository;
    }

    public async Task<Result<long>> Handle(DispatchMessageCommand request,
                                           CancellationToken      cancellationToken) {
        var message = new Message {
            Id        = _snowflakeGen.GetNewSnowflakeId(),
            Content   = request.Content,
            ChannelId = request.ChannelId
        };
        await _messageRepository.CreateAsync(message, cancellationToken);
        await DispatchToConnectedUsersAsync(request.Recipients, request.Tag, message, cancellationToken);

        return message.Id;
    }
}
