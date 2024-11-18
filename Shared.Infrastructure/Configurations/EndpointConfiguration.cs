namespace Shared.Infrastructure.Configurations;

public static class EndpointConfiguration
{
    public static IServiceCollection AddEndpointConfiguration(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static IEndpointRouteBuilder UseEndpointConfiguration(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            //.HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();

        var endpointGroup = app.MapGroup("api/v{apiVersion:apiVersion}")
            .WithApiVersionSet(versionSet)
            .WithOpenApi();

        return endpointGroup;
    }
}