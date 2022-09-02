using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Monads;

namespace Oleexo.RealtimeDistributedSystem.Common.Commands;

public interface ICommandBase {
}

public interface ICommand : ICommand<Unit> {
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandBase {
}
