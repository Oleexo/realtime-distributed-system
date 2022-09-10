using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Store.SnowflakeGen;

namespace Oleexo.RealtimeDistributedSystem.Store.Commands.StoreMessage;

public sealed class StoreMessageCommandHandler : ICommandHandler<StoreMessageCommand, long> {
    private readonly ISnowflakeGen      _snowflakeGen;
    private readonly IMessageRepository _messageRepository;

    public StoreMessageCommandHandler(ISnowflakeGen      snowflakeGen,
                                      IMessageRepository messageRepository) {
        _snowflakeGen      = snowflakeGen;
        _messageRepository = messageRepository;
    }

    public async Task<Result<long>> Handle(StoreMessageCommand request,
                                           CancellationToken   cancellationToken) {
        var message = new Message {
            Id        = _snowflakeGen.GetNewSnowflakeId(),
            Content   = request.Content,
            ChannelId = request.ChannelId
        };
        await _messageRepository.CreateAsync(message, cancellationToken);
        return message.Id;
    }
}
