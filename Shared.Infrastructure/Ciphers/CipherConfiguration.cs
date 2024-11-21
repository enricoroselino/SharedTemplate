using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Ciphers.Algorithms.Aes;

namespace Shared.Infrastructure.Ciphers;

public static class CipherConfiguration
{
    public static IServiceCollection AddCiphers(this IServiceCollection services)
    {
        services
            .AddTransient<ITextCipher, TextCipher>()
            .AddTransient<IFileCipher, FileCipher>()
            .AddOptions<AesCipherOptions>()
            .BindConfiguration(AesCipherOptions.Section);

        return services;
    }
}