using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Infrastructure.Configurations.Documentation;

public class SwaggerVersioningOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public SwaggerVersioningOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var infoDescription = description.IsDeprecated ? "API version is deprecated" : "API Documentation";
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description, infoDescription));
        }
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description, string infoDescription)
    {
        var openApiInfo = new OpenApiInfo
        {
            Title = "Shared Template API",
            Version = description.ApiVersion.ToString(),
            Description = infoDescription,
        };

        return openApiInfo;
    }
}