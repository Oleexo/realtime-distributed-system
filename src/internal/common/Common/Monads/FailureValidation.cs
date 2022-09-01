namespace Oleexo.RealtimeDistributedSystem.Common.Monads;

internal sealed class FailureValidation<TFail, TSuccess> : Validation<TFail, TSuccess>
{
    private readonly IReadOnlyCollection<TFail> _fails;

    public FailureValidation(IReadOnlyCollection<TFail> fails)
    {
        _fails = fails;
    }

    public override bool IsFail    => true;
    public override bool IsSuccess => false;

    public override void Match(Action<TSuccess>                   success,
                               Action<IReadOnlyCollection<TFail>> failure)
    {
        failure(_fails);
    }

    public override TResponse Match<TResponse>(Func<TSuccess, TResponse>                   success,
                                               Func<IReadOnlyCollection<TFail>, TResponse> failure)
    {
        return failure(_fails);
    }

    public override TSuccess IfFail(Func<TSuccess> failure)
    {
        return failure();
    }

    public override TSuccess IfFail(Func<IReadOnlyCollection<TFail>, TSuccess> failure)
    {
        return failure(_fails);
    }

    public override void IfSuccess(Action<TSuccess> success)
    {
    }
}
