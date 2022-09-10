using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Exceptions;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Common.Queries;

namespace Oleexo.RealtimeDistributedSystem.Store.Queries.RetrieveMessage;

public sealed class RetrieveMessageQueryHandler : IQueryHandler<RetrieveMessageQuery, Message> {
    private readonly IMessageRepository _messageRepository;

    public RetrieveMessageQueryHandler(IMessageRepository messageRepository) {
        _messageRepository = messageRepository;
    }

    public async Task<Result<Message>> Handle(RetrieveMessageQuery request,
                                               CancellationToken   cancellationToken) {
        var message = await _messageRepository.GetByIdAsync(request.ChannelId, request.MessageId, cancellationToken);
        if (message is null) {
            return new MessageNotFoundException(request.ChannelId, request.MessageId);
        }

        return message;
    }
}
