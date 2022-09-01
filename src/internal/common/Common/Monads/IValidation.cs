namespace Oleexo.RealtimeDistributedSystem.Common.Monads;

public interface IValidation<TFail, TSuccess>
{
    bool IsFail    { get; }
    bool IsSuccess { get; }

    void Match(Action<TSuccess>                   success,
               Action<IReadOnlyCollection<TFail>> failure);

    TResponse Match<TResponse>(Func<TSuccess, TResponse>                   success,
                               Func<IReadOnlyCollection<TFail>, TResponse> failure);

    TSuccess IfFail(Func<TSuccess>                             failure);
    TSuccess IfFail(Func<IReadOnlyCollection<TFail>, TSuccess> failure);
    void IfSuccess(Action<TSuccess>                            success);
}
