namespace Shared.Documentation;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerAuthentication()
            .ConfigureOptions<SwaggerVersioningOptions>();

        return services;
    }

    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in app.DescribeApiVersions())
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });
        
        return app;
    }

    private static IServiceCollection AddSwaggerAuthentication(this IServiceCollection services)
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