using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchMessage;

public sealed class DispatchMessageCommandHandler : ICommandHandler<DispatchMessageCommand, long> {
    private readonly IBrokerPusher             _brokerPusher;
    private readonly IUserConnectionRepository _userConnectionRepository;

    public DispatchMessageCommandHandler(IBrokerPusher             brokerPusher,
                                         IUserConnectionRepository userConnectionRepository) {
        _brokerPusher             = brokerPusher;
        _userConnectionRepository = userConnectionRepository;
    }

    public async Task<Result<long>> Handle(DispatchMessageCommand request,
                                           CancellationToken      cancellationToken) {
        var recipients = new HashSet<string>(request.Recipients);
        // todo save the message
        var connectedUsers = await _userConnectionRepository.GetConnectedUsersWithTag(request.Tag, cancellationToken);
        // todo find user online based on Recipients
        var connectedRecipients = connectedUsers.Where(p => recipients.Contains(p.UserId))
                                                .GroupBy(p => p.Queue)
                                                .ToArray();
        var messageWrapper = new MessageWrapper {
            Message = new Message {
                Id      = 1,
                Content = request.Content
            },
            Tags = new[] { request.Tag }
        };
        foreach (var usersByQueue in connectedRecipients) {
            await _brokerPusher.PushMessageAsync(messageWrapper with {
                Recipients = usersByQueue.Select(x => x.UserId).ToArray()
            }, usersByQueue.Key, cancellationToken);
        }

        // todo send message in queue of pusher server with list of users filtered
        return 1;
    }
}
