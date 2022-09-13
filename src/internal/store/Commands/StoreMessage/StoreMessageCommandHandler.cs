using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Store.Domain.Events;
using Oleexo.RealtimeDistributedSystem.Store.SnowflakeGen;

namespace Oleexo.RealtimeDistributedSystem.Store.Commands.StoreMessage;

public sealed class StoreMessageCommandHandler : ICommandHandler<StoreMessageCommand, long> {
    private readonly IMessageRepository _messageRepository;
    private readonly IPublisher         _publisher;
    private readonly ISnowflakeGen      _snowflakeGen;

    public StoreMessageCommandHandler(ISnowflakeGen      snowflakeGen,
                                      IMessageRepository messageRepository,
                                      IPublisher         publisher) {
        _snowflakeGen      = snowflakeGen;
        _messageRepository = messageRepository;
        _publisher         = publisher;
    }

    public async Task<Result<long>> Handle(StoreMessageCommand request,
                                           CancellationToken   cancellationToken) {
        var message = new Message {
            Id        = _snowflakeGen.GetNewSnowflakeId(),
            Content   = request.Content,
            ChannelId = request.ChannelId
        };
        await _messageRepository.CreateAsync(message, cancellationToken);
        await _publisher.Publish(new MessageCreated {
            Message    = message,
            Recipients = request.Recipients,
            Tag        = request.Tag
        }, cancellationToken);
        return message.Id;
    }
}
