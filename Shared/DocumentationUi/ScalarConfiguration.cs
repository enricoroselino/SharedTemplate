namespace Shared.Documentation;

public static class ScalarConfiguration
{
    public static IServiceCollection AddScalarConfiguration(this IServiceCollection services)
    {
        services.AddOpenApi();
        return services;
    }

    public static WebApplication UseScalarConfiguration(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;

        app.MapScalarApiReference();
        app.MapOpenApi();
        return app;
    }
}