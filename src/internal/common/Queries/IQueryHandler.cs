using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Monads;

namespace Oleexo.RealtimeDistributedSystem.Common.Queries;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>, IRequest<Result<TResponse>> {
}
