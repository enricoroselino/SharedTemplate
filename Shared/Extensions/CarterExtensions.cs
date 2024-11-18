namespace Shared.Extensions;

public static class CarterExtensions
{
    public static IServiceCollection AddCarterFromAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddCarter(configurator: Configurator);
        return services;

        void Configurator(CarterConfigurator cfg)
        {
            var modules = AssembliesHelper.GetInterfaceTypes<ICarterModule>(assemblies);
            cfg.WithModules(modules);
        }
    }
}