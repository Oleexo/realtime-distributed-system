using System.Diagnostics.Contracts;

namespace Oleexo.RealtimeDistributedSystem.Common.Monads;

internal class SuccessResult<TValue> : Result<TValue>
{
    private readonly TValue _value;

    public SuccessResult(TValue value)
    {
        _value = value;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void IfFail(Action<Exception> f)
    {
    }

    public override void IfSuccess(Action<TValue> f)
    {
        f(_value);
    }

    [Pure]
    public override Result<TReturnedValue> Map<TReturnedValue>(Func<TValue, TReturnedValue> f)
    {
        return new SuccessResult<TReturnedValue>(f(_value));
    }

    [Pure]
    public override async Task<IResult<TReturnedValue>> MapAsync<TReturnedValue>(Func<TValue, Task<TReturnedValue>> f)
    {
        var result = await f(_value);
        return new SuccessResult<TReturnedValue>(result);
    }

    [Pure]
    public override TResponse Match<TResponse>(Func<TValue, TResponse>    success,
                                               Func<Exception, TResponse> failure)
    {
        return success(_value);
    }

    public override TValue    Value => _value;
    public override Exception Error => throw new InvalidOperationException("No error in success context");
}
