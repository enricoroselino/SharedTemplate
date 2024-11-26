using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Ciphers;
using Shared.Infrastructure.Configurations;
using Shared.Infrastructure.Configurations.Documentation;
using Shared.Infrastructure.Providers;
using Shared.Infrastructure.Providers.GuidProvider;
using Shared.Persistence.Extensions;
using Shared.Persistence.Interceptors;

namespace Shared;

public static class SharedConfiguration
{
    public static IServiceCollection AddSharedConfiguration(this IServiceCollection services)
    {
        services
            .AddCarter()
            .AddSerilogConfig()
            .AddQuartzConfiguration()
            .AddSwaggerDocumentation();

        services
            .AddEndpointConfiguration()
            .AddAuthenticationConfiguration()
            .AddAuthorization();

        services
            .AddAntiforgery()
            .Configure<KestrelServerOptions>(options => { options.Limits.MaxRequestBodySize = 26 * 1024 * 1024; });

        services
            .AddCors()
            .AddProblemDetails()
            .AddExceptionHandler<GlobalExceptionHandler>();

        services
            .AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>()
            .AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services
            .AddTransient<IGuidProviderFactory, GuidProviderFactory>()
            .AddTransient<MssqlGuidProvider>()
            .AddTransient<GuidProvider>()
            .AddSingleton<TimeProvider>(TimeProvider.System);

        services.AddCiphers();

        return services;
    }

    public static WebApplication UseSharedConfiguration(this WebApplication app)
    {
        app.UseSwaggerDocumentation();

        app
            .UseHttpsRedirection()
            .UseAntiforgery()
            .UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
            .UseExceptionHandler(cfg => { });

        app
            .UseAuthentication()
            .UseAuthorization();

        app
            .UseEndpointConfiguration()
            .MapCarter();

        Task.Run(async () => await app.SeedDatabaseAsync());
        return app;
    }
}