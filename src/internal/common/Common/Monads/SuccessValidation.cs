namespace Oleexo.RealtimeDistributedSystem.Common.Monads;

internal sealed class SuccessValidation<TFail, TSuccess> : Validation<TFail, TSuccess>
{
    private readonly TSuccess _value;

    public SuccessValidation(TSuccess value)
    {
        _value = value;
    }

    public override bool IsFail    => false;
    public override bool IsSuccess => true;

    public override void Match(Action<TSuccess>                   success,
                               Action<IReadOnlyCollection<TFail>> failure)
    {
        success(_value);
    }

    public override TResponse Match<TResponse>(Func<TSuccess, TResponse>                   success,
                                               Func<IReadOnlyCollection<TFail>, TResponse> failure)
    {
        return success(_value);
    }

    public override TSuccess IfFail(Func<TSuccess> failure)
    {
        return _value;
    }

    public override TSuccess IfFail(Func<IReadOnlyCollection<TFail>, TSuccess> failure)
    {
        return _value;
    }

    public override void IfSuccess(Action<TSuccess> success)
    {
        success(_value);
    }
}
