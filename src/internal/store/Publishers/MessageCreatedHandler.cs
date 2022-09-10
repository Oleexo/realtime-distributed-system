using MassTransit;
using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.BusMessages;
using Oleexo.RealtimeDistributedSystem.Store.Domain.Events;

namespace Oleexo.RealtimeDistributedSystem.Store.Publishers;

public sealed class MessageCreatedHandler : INotificationHandler<MessageCreated> {
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public MessageCreatedHandler(ISendEndpointProvider sendEndpointProvider) {
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Handle(MessageCreated    notification,
                             CancellationToken cancellationToken) {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:message"));
        await endpoint.Send(new DispatchMessage {
            Message    = notification.Message,
            Recipients = notification.Recipients,
            Tag        = notification.Tag
        }, cancellationToken);
    }
}
