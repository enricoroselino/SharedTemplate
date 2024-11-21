using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Behaviors;

namespace Shared.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediatorFromAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // validation added as part of mediator
        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }
}