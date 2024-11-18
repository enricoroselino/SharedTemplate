using Microsoft.OpenApi.Models;

namespace Shared.Extensions;

internal static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerAuthentication(this IServiceCollection services)
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            In = ParameterLocation.Header,
            BearerFormat = "JWT",
            Scheme = "bearer",
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            },
            Description = "Please enter your JWT with this format: ''YOUR_TOKEN''",
        };

        var securityRequirement = new OpenApiSecurityRequirement { { securityScheme, Array.Empty<string>() } };

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", securityScheme);
            options.AddSecurityRequirement(securityRequirement);
        });
        
        return services;
    }
}