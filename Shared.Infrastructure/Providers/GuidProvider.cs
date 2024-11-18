namespace Shared.Infrastructure.Providers;

public interface IGuidProvider
{
    public Guid NewRandom();
    public Guid NewSequential();
}

public class GuidProvider : IGuidProvider
{
    public Guid NewRandom() => Guid.NewGuid();
    public Guid NewSequential() => Uuid.NewSequential();
}