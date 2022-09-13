using MassTransit;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher.MassTransit;

internal sealed class MassTransitBrokerPush : BaseBrokerPusher {
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public MassTransitBrokerPush(ISendEndpointProvider sendEndpointProvider) {
        _sendEndpointProvider = sendEndpointProvider;
    }

    protected override bool IsSupported(QueueType queueType) {
        return true;
    }

    protected override async Task SendMessageAsync(string            content,
                                                   string            queueName,
                                                   CancellationToken cancellationToken = default) {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(queueName));
        await endpoint.Send(content, cancellationToken);
    }
}
