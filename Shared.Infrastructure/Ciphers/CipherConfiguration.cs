using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Ciphers;

public static class CipherConfiguration
{
    public static IServiceCollection AddCiphers(this IServiceCollection services)
    {
        services
            .AddTransient<ITextCipher, TextCipher>()
            .AddOptions<CipherOptions>()
            .BindConfiguration(CipherOptions.Section);

        return services;
    }
}