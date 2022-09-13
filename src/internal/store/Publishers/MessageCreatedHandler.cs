using MassTransit;
using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Store.Domain.Events;

namespace Oleexo.RealtimeDistributedSystem.Store.Publishers;

public sealed class MessageCreatedHandler : INotificationHandler<MessageCreated> {
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public MessageCreatedHandler(ISendEndpointProvider sendEndpointProvider) {
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Handle(MessageCreated    notification,
                             CancellationToken cancellationToken) {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:mailbox"));
        
        
        await endpoint.Send(new Letter {
            Message    = notification.Message,
            Tag        = notification.Tag,
            Type       = LetterType.Message,
            Recipients = notification.Recipients
        }, cancellationToken);
    }
}