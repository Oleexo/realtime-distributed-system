using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Monads;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.Common;

public abstract class BaseDispatch {
    private readonly IBrokerPusher             _brokerPusher;
    private readonly IUserConnectionRepository _userConnectionRepository;

    protected BaseDispatch(IBrokerPusher             brokerPusher,
                           IUserConnectionRepository userConnectionRepository) {
        _brokerPusher             = brokerPusher;
        _userConnectionRepository = userConnectionRepository;
    }

    protected Task DispatchToConnectedUsersAsync(IReadOnlyCollection<string> recipients,
                                                 string                      tag,
                                                 Message                     message,
                                                 CancellationToken           cancellationToken = default) {
        var letter = new Letter {
            Message = message,
            Type    = LetterType.Message,
            Tag     = tag
        };
        return DispatchToConnectedUsersAsync(letter, recipients, cancellationToken);
    }

    protected Task DispatchToConnectedUsersAsync(IReadOnlyCollection<string> recipients,
                                                 string                      tag,
                                                 Event                       @event,
                                                 CancellationToken           cancellationToken = default) {
        var letter = new Letter {
            Event = @event,
            Type  = LetterType.Event,
            Tag   = tag
        };
        return DispatchToConnectedUsersAsync(letter, recipients, cancellationToken);
    }

    protected async Task<Result<Unit>> DispatchToConnectedUsersAsync(Letter                      letter,
                                                                     IReadOnlyCollection<string> recipients,
                                                                     CancellationToken           cancellationToken = default) {
        var recipientSet   = new HashSet<string>(recipients);
        var connectedUsers = await _userConnectionRepository.GetConnectedUsersWithTag(letter.Tag, cancellationToken);
        var connectedRecipients = connectedUsers.Where(p => recipientSet.Contains(p.UserId))
                                                .GroupBy(p => p.Queue)
                                                .ToArray();
        foreach (var usersByQueue in connectedRecipients) {
            await _brokerPusher.PushAsync(letter with {
                Recipients = usersByQueue.Select(x => x.UserId)
                                         .ToArray()
            }, usersByQueue.Key, cancellationToken);
        }

        return Result<Unit>.Success(Unit.Value);
    }
}
