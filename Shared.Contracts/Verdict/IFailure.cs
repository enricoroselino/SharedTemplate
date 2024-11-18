namespace Shared.Contracts.Verdict;

public interface IFailure
{
    public FailureType Type { get; }
    public string Message { get; }
}