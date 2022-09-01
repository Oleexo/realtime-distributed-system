namespace Oleexo.RealtimeDistributedSystem.Common.Monads;

public abstract class Result<TValue> : IResult<TValue>
{
    public abstract bool IsFaulted { get; }
    public abstract bool IsSuccess { get; }
    public abstract void IfFail(Action<Exception>                                                             f);
    public abstract void IfSuccess(Action<TValue>                                                             f);
    public abstract IResult<TReturnedValue> Map<TReturnedValue>(Func<TValue, TReturnedValue>                  f);
    public abstract Task<IResult<TReturnedValue>> MapAsync<TReturnedValue>(Func<TValue, Task<TReturnedValue>> f);

    public abstract TResponse Match<TResponse>(Func<TValue, TResponse>    success,
                                               Func<Exception, TResponse> failure);
    
    public abstract TValue    Value { get; }
    public abstract Exception Error { get; }

    public static implicit operator Result<TValue>(TValue value)
    {
        return new SuccessResult<TValue>(value);
    }

    public static implicit operator Result<TValue>(Exception exception)
    {
        return new FailureResult<TValue>(exception);
    }

    public static Result<TValue> Success(TValue value)
    {
        return new SuccessResult<TValue>(value);
    }

    public static Result<TValue> Fail(Exception exception)
    {
        return new FailureResult<TValue>(exception);
    }
}
