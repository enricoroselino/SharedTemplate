namespace Shared.Infrastructure.Providers.GuidProvider;

public interface IGuidProvider
{
    public Guid NewRandom();
    public Guid NewSequential();
}