namespace Shared.Contracts.Verdict;

public class Verdict : IVerdict
{
    protected Verdict(bool isSuccess, IFailure failure)
    {
        if (isSuccess && !Equals(failure, Contracts.Verdict.Failure.None) ||
            !isSuccess && Equals(failure, Contracts.Verdict.Failure.None))
        {
            throw new VerdictException("Invalid Failure", nameof(failure));
        }

        IsSuccess = isSuccess;
        Failure = failure;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IFailure Failure { get; }
    public static Verdict Success() => new Verdict(true, failure: Contracts.Verdict.Failure.None);
    public static Verdict<T> Success<T>(T value) => new Verdict<T>(true, value, Contracts.Verdict.Failure.None);
    public static Verdict Failed(IFailure failure) => new Verdict(false, failure: failure);
    public static Verdict<T> Failed<T>(IFailure failure) => new Verdict<T>(false, default, failure);

    public TResult Fold<TResult>(Func<TResult> onSuccess, Func<IFailure, TResult> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(Failure);
    }
}

public class Verdict<T> : Verdict, IVerdict<T>
{
    internal Verdict(bool isSuccess, T? data, IFailure failure) : base(isSuccess, failure)
    {
        Data = data;
    }

    public T? Data { get; }

    public TResult Fold<TResult>(Func<T?, TResult> onSuccess, Func<IFailure, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Data) : onFailure(Failure);
    }
}