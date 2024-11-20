namespace Shared;

public static class SharedConfiguration
{
    public static IServiceCollection AddSharedConfiguration(this IServiceCollection services)
    {
        services
            .AddCarter()
            .AddSerilogConfig()
            .AddQuartzConfiguration()
            .AddEndpointConfiguration()
            .AddAuthenticationConfiguration();

        services
            .AddCors()
            .AddProblemDetails()
            .AddExceptionHandler<GlobalExceptionHandler>();

        services
            .AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>()
            .AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services
            .AddSingleton<IGuidProvider, GuidProvider>()
            .AddSingleton<TimeProvider>(TimeProvider.System);

        return services;
    }

    public static WebApplication UseSharedConfiguration(this WebApplication app)
    {
        app
            .UseHttpsRedirection()
            .UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
            .UseExceptionHandler(cfg => { });

        app
            .UseEndpointConfiguration()
            .MapCarter();

        Task.Run(async () => await app.SeedDatabaseAsync());
        return app;
    }
}