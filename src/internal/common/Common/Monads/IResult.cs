namespace Oleexo.RealtimeDistributedSystem.Common.Monads;

public interface IResult<TValue>
{
    bool IsFaulted { get; }
    bool IsSuccess { get; }
    void IfFail(Action<Exception>                                                             f);
    void IfSuccess(Action<TValue>                                                             f);
    IResult<TReturnedValue> Map<TReturnedValue>(Func<TValue, TReturnedValue>                  f);
    Task<IResult<TReturnedValue>> MapAsync<TReturnedValue>(Func<TValue, Task<TReturnedValue>> f);

    TResponse Match<TResponse>(Func<TValue, TResponse>    success,
                               Func<Exception, TResponse> failure);
}
