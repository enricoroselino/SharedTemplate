namespace Shared.Infrastructure.Configurations;

public static class AuthenticationConfiguration
{
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
    {
        services
            .AddSingleton<ITokenProvider, TokenProvider>()
            .AddOptions<TokenOptions>()
            .BindConfiguration(TokenOptions.Section);

        using var serviceProvider = services.BuildServiceProvider();
        var tokenProvider = serviceProvider.GetRequiredService<ITokenProvider>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = tokenProvider.Scheme;
                options.DefaultChallengeScheme = tokenProvider.Scheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokenProvider.TokenValidationParameters;
            });

        return services;
    }
}