using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Store.SnowflakeGen;

namespace Oleexo.RealtimeDistributedSystem.Store.Commands.EditMessage;

public sealed class EditMessageCommandHandler : ICommandHandler<EditMessageCommand, long> {
    private readonly ISnowflakeGen      _snowflakeGen;
    private readonly IMessageRepository _messageRepository;

    public EditMessageCommandHandler(ISnowflakeGen      snowflakeGen,
                                     IMessageRepository messageRepository) {
        _snowflakeGen      = snowflakeGen;
        _messageRepository = messageRepository;
    }

    public async Task<Result<long>> Handle(EditMessageCommand request,
                                           CancellationToken  cancellationToken) {
        var message = await _messageRepository.GetByIdAsync(request.ChannelId, request.MessageId, cancellationToken);
        if (message is null) {
            return new InvalidOperationException("Message not found");
        }

        if (message.ParentId.HasValue) {
            return new InvalidOperationException("Cannot edit a edition");
        }

        var editionMessage = message with {
            Content = request.Content,
            Id = _snowflakeGen.GetNewSnowflakeId(),
            ParentId = message.Id
        };
        await _messageRepository.CreateAsync(editionMessage, cancellationToken);
        return message.Id;
    }
}
