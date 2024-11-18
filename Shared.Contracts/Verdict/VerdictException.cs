namespace Shared.Contracts.Verdict;

public class VerdictException : Exception
{
    public VerdictException(string message) : base(message)
    {
    }

    public VerdictException(string message, string details) : base(message)
    {
        Details = details;
    }

    public string? Details { get; }
}