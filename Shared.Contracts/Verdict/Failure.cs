namespace Shared.Contracts.Verdict;

public sealed class Failure : IEquatable<Failure>, IFailure
{
    private Failure(string message, FailureType type)
    {
        Message = message;
        Type = type;
    }

    public FailureType Type { get; }
    public string Message { get; }

    public static IFailure None => new Failure(string.Empty, FailureType.None);
    public static IFailure BadRequest(string message) => new Failure(message, FailureType.BadRequest);
    public static IFailure Forbidden(string message) => new Failure(message, FailureType.Forbidden);
    public static IFailure NotFound(string message) => new Failure(message, FailureType.NotFound);
    public static IFailure Conflict(string message) => new Failure(message, FailureType.Conflict);
    public static IFailure Server() => new Failure("Something just crashed.", FailureType.Server);

    public bool Equals(Failure? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Message, other.Message, StringComparison.InvariantCultureIgnoreCase) &&
               Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Failure other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Message, StringComparer.InvariantCultureIgnoreCase);
        hashCode.Add((int)Type);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(Failure? left, Failure? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Failure? left, Failure? right)
    {
        return !Equals(left, right);
    }
}