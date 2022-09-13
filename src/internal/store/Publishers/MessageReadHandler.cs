using MassTransit;
using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Store.Domain.Events;
using Event = Oleexo.RealtimeDistributedSystem.Common.Domain.Entities.Event;
namespace Oleexo.RealtimeDistributedSystem.Store.Publishers;

public sealed class MessageReadHandler : INotificationHandler<MessageRead> {
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public MessageReadHandler(ISendEndpointProvider sendEndpointProvider) {
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Handle(MessageRead       notification,
                             CancellationToken cancellationToken) {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:mailbox"));
        await endpoint.Send(new Letter {
            Type = LetterType.Event,
            Recipients = notification.Recipients,
            Tag = notification.Tag,
            Event = new Event {
                Author = notification.UserId,
                Content = "Message read TBD"
            }
        }, cancellationToken);
    }
}
