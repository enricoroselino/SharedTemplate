namespace Shared.Documentation;

public static class ScalarConfiguration
{
    public static IServiceCollection AddScalarDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi();
        return services;
    }

    public static WebApplication UseScalarDocumentation(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;

        app.MapScalarApiReference();
        app.MapOpenApi();
        return app;
    }
}