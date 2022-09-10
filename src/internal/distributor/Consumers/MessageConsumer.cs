using MassTransit;
using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.BusMessages;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchMessage;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Consumers;

public sealed class MessageConsumer : IConsumer<DispatchMessage> {
    private readonly IMediator _mediator;

    public MessageConsumer(IMediator mediator) {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<DispatchMessage> context) {
        var content = context.Message;
        return _mediator.Send(new DispatchMessageCommand {
            Message    = content.Message,
            Recipients = content.Recipients,
            Tag        = content.Tag
        });
    }
}

public sealed class MessageConsumerDefinition : ConsumerDefinition<MessageConsumer> {
    public MessageConsumerDefinition() {
        EndpointName = "message";
    }
}
