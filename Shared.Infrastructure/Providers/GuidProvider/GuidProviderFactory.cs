using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Enums;

namespace Shared.Infrastructure.Providers.GuidProvider;

public interface IGuidProviderFactory
{
    public IGuidProvider Create(DatabaseFlavor flavor);
}

public class GuidProviderFactory : IGuidProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public GuidProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IGuidProvider Create(DatabaseFlavor flavor)
    {
        IGuidProvider provider = flavor switch
        {
            DatabaseFlavor.Mssql => _serviceProvider.GetRequiredService<MssqlGuidProvider>(),
            _ => _serviceProvider.GetRequiredService<GuidProvider>()
        };

        return provider;
    }
}