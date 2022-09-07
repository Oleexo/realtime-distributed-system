using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.Common;
using Oleexo.RealtimeDistributedSystem.Distributor.SnowflakeGen;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.DeleteMessage;

public sealed class DeleteMessageCommandHandler : BaseDispatch, ICommandHandler<DeleteMessageCommand, long> {
    private readonly IMessageRepository _messageRepository;
    private readonly ISnowflakeGen      _snowflakeGen;

    public DeleteMessageCommandHandler(IMessageRepository        messageRepository,
                                       ISnowflakeGen             snowflakeGen,
                                       IBrokerPusher             brokerPusher,
                                       IUserConnectionRepository userConnectionRepository)
        : base(brokerPusher, userConnectionRepository) {
        _messageRepository = messageRepository;
        _snowflakeGen      = snowflakeGen;
    }

    public async Task<Result<long>> Handle(DeleteMessageCommand request,
                                           CancellationToken    cancellationToken) {
        var message = await _messageRepository.GetByIdAsync(request.ChannelId, request.MessageId, cancellationToken);
        if (message is null) {
            return new InvalidOperationException("Message not found");
        }

        if (message.ParentId.HasValue) {
            return new InvalidOperationException("Cannot delete a edition");
        }
        var deletionMessage = message with {
            Content = string.Empty,
            Id = _snowflakeGen.GetNewSnowflakeId(),
            ParentId = message.Id,
            IsDeletion = true
        };
        await _messageRepository.CreateAsync(deletionMessage, cancellationToken);
        await DispatchToConnectedUsersAsync(request.Recipients, request.Tag, deletionMessage, cancellationToken);
        return deletionMessage.Id;

    }
}
