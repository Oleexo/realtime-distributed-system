namespace Oleexo.RealtimeDistributedSystem.Common.Monads;

public abstract class Validation<TFail, TSuccess> : IValidation<TFail, TSuccess>
{
    public abstract bool IsFail    { get; }
    public abstract bool IsSuccess { get; }

    public abstract void Match(Action<TSuccess>                   success,
                               Action<IReadOnlyCollection<TFail>> failure);

    public abstract TResponse Match<TResponse>(Func<TSuccess, TResponse>                   success,
                                               Func<IReadOnlyCollection<TFail>, TResponse> failure);

    public abstract TSuccess IfFail(Func<TSuccess>                             failure);
    public abstract TSuccess IfFail(Func<IReadOnlyCollection<TFail>, TSuccess> failure);
    public abstract void IfSuccess(Action<TSuccess>                            success);

    public static implicit operator Validation<TFail, TSuccess>(TSuccess value)
    {
        return new SuccessValidation<TFail, TSuccess>(value);
    }

    public static Validation<TFail, TSuccess> Success(TSuccess value)
    {
        return new SuccessValidation<TFail, TSuccess>(value);
    }

    public static Validation<TFail, TSuccess> Fail(IReadOnlyCollection<TFail> fails)
    {
        return new FailureValidation<TFail, TSuccess>(fails);
    }
}
