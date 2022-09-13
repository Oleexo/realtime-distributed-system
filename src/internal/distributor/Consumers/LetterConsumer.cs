using MassTransit;
using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.Dispatch;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Consumers;

public sealed class LetterConsumer : IConsumer<Letter> {
    private readonly IMediator _mediator;

    public LetterConsumer(IMediator mediator) {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<Letter> context) {
        return _mediator.Send(new DispatchLetterCommand {
            Letter = context.Message
        });
    }
}

public sealed class LetterConsumerDefinition : ConsumerDefinition<LetterConsumer> {
    public LetterConsumerDefinition() {
        EndpointName = "mailbox";
    }
}
