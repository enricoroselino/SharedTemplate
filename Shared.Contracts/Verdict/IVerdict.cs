namespace Shared.Contracts.Verdict;

public interface IVerdict
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public IFailure Failure { get; }
    public TResult Fold<TResult>(Func<TResult> onSuccess, Func<IFailure, TResult> onFailure);
}

public interface IVerdict<out T> : IVerdict
{
    public T? Data { get; }
    public TResult Fold<TResult>(Func<T?, TResult> onSuccess, Func<IFailure, TResult> onFailure);
}