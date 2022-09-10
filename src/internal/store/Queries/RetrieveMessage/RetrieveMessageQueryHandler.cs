using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Common.Queries;

namespace Oleexo.RealtimeDistributedSystem.Store.Queries.GetMessage;

public sealed class RetrieveMessageQueryHandler : IQueryHandler<RetrieveMessageQuery, Message?> {
    private readonly IMessageRepository _messageRepository;

    public RetrieveMessageQueryHandler(IMessageRepository messageRepository) {
        _messageRepository = messageRepository;
    }

    public async Task<Result<Message?>> Handle(RetrieveMessageQuery request,
                                               CancellationToken   cancellationToken) {
        return await _messageRepository.GetByIdAsync(request.ChannelId, request.MessageId, cancellationToken);
    }
}
