using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.Common;
using Oleexo.RealtimeDistributedSystem.Distributor.SnowflakeGen;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.EditMessage;

public sealed class EditMessageCommandHandler : BaseDispatch, ICommandHandler<EditMessageCommand, long> {
    private readonly IMessageRepository _messageRepository;
    private readonly ISnowflakeGen      _snowflakeGen;

    public EditMessageCommandHandler(IMessageRepository        messageRepository,
                                     ISnowflakeGen snowflakeGen,
                                     IBrokerPusher             brokerPusher,
                                     IUserConnectionRepository userConnectionRepository)
        : base(brokerPusher, userConnectionRepository) {
        _messageRepository = messageRepository;
        _snowflakeGen = snowflakeGen;
    }

    public async Task<Result<long>> Handle(EditMessageCommand request,
                                           CancellationToken  cancellationToken) {
        var message = await _messageRepository.GetByIdAsync(request.ChannelId, request.MessageId);
        if (message is null) {
            return new InvalidOperationException("Message not found");
        }

        var editMessage = message with {
            Content = request.Content,
            Id = _snowflakeGen.GetNewSnowflakeId(),
            ParentId = message.Id
        };
        await _messageRepository.CreateAsync(editMessage, cancellationToken);
        await DispatchToConnectedUsersAsync(request.Recipients, request.Tag, editMessage, cancellationToken);
        return editMessage.Id;
    }
}
