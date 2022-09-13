using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Store.Domain.Events;

namespace Oleexo.RealtimeDistributedSystem.Store.Commands.ReadMessage;

public sealed class ReadMessageCommandHandler : ICommandHandler<ReadMessageCommand> {
    private readonly IChannelRepository _channelRepository;
    private readonly IMediator          _mediator;

    public ReadMessageCommandHandler(IChannelRepository channelRepository,
                                     IMediator          mediator) {
        _channelRepository = channelRepository;
        _mediator          = mediator;
    }

    public async Task<Result<Unit>> Handle(ReadMessageCommand request,
                                           CancellationToken  cancellationToken) {
        var channel = await _channelRepository.GetByIdAsync(request.ChannelId, cancellationToken) ?? new Channel();

        if (!channel.UserInfos.TryGetValue(request.UserId, out var userInfo)) {
            userInfo = new ChannelUserInfo();
            channel.UserInfos.Add(request.UserId, userInfo);
        }

        userInfo.LastMessageRead = request.MessageId;
        await _mediator.Publish(new MessageRead {
            ChannelId  = request.ChannelId,
            MessageId  = request.MessageId,
            UserId     = request.UserId,
            Recipients = request.Recipients,
            Tag        = request.Tag
        }, cancellationToken);
        return Result<Unit>.Success(Unit.Value);
    }
}
