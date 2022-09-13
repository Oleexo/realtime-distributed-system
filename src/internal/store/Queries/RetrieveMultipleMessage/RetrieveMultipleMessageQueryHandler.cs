using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Common.Queries;

namespace Oleexo.RealtimeDistributedSystem.Store.Queries.RetrieveMultipleMessage;

public sealed class RetrieveMultipleMessageQueryHandler : IQueryHandler<RetrieveMultipleMessageQuery, IReadOnlyCollection<Message>> {
    private readonly IMessageRepository _messageRepository;

    public RetrieveMultipleMessageQueryHandler(IMessageRepository messageRepository) {
        _messageRepository = messageRepository;
    }

    public async Task<Result<IReadOnlyCollection<Message>>> Handle(RetrieveMultipleMessageQuery request,
                                                                   CancellationToken            cancellationToken) {
        var messages = await _messageRepository.GetAllAsync(request.ChannelId, new GetAllOptions {
            Limit       = request.Limit,
            Offset      = request.Offset,
            EventStyle  = request.EventStyle,
            ScanForward = request.ScanForward
        }, cancellationToken);

        return Result<IReadOnlyCollection<Message>>.Success(messages);
    }
}
