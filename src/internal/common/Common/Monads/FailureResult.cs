using System.Diagnostics.Contracts;

namespace Oleexo.RealtimeDistributedSystem.Common.Monads;

internal class FailureResult<TValue> : Result<TValue>
{
    private readonly Exception _exception;

    public FailureResult(Exception exception)
    {
        _exception = exception;
    }

    public override bool IsFaulted => true;
    public override bool IsSuccess => false;

    public override void IfFail(Action<Exception> f)
    {
        f(_exception);
    }

    public override void IfSuccess(Action<TValue> f)
    {
    }

    [Pure]
    public override IResult<TReturnedValue> Map<TReturnedValue>(Func<TValue, TReturnedValue> f)
    {
        return new FailureResult<TReturnedValue>(_exception);
    }

    [Pure]
    public override Task<IResult<TReturnedValue>> MapAsync<TReturnedValue>(Func<TValue, Task<TReturnedValue>> f)
    {
        return Task.FromResult<IResult<TReturnedValue>>(new FailureResult<TReturnedValue>(_exception));
    }

    [Pure]
    public override TResponse Match<TResponse>(Func<TValue, TResponse>    success,
                                               Func<Exception, TResponse> failure)
    {
        return failure(_exception);
    }

    public override void Match(Action<TValue>    success,
                               Action<Exception> failure) {
        failure(_exception);
    }

    public override TValue    Value => throw new InvalidOperationException("No value in error context");
    public override Exception Error => _exception;
}
