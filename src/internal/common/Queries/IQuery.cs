using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Monads;

namespace Oleexo.RealtimeDistributedSystem.Common.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IQueryBase {
}
