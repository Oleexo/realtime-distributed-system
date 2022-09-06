using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Monads;

namespace Oleexo.RealtimeDistributedSystem.Common.Commands;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>, IRequest<Result<TResponse>> {
}

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result<Unit>>
    where TCommand : ICommand, IRequest<Result<Unit>> {
}