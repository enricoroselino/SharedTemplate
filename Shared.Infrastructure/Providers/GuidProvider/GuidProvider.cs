namespace Shared.Infrastructure.Providers.GuidProvider;

public class GuidProvider : IGuidProvider
{
    public Guid NewRandom() => Guid.NewGuid();
    public Guid NewSequential() => Guid.CreateVersion7();
}