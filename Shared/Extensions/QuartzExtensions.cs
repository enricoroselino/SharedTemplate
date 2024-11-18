using Microsoft.Extensions.Options;
using Quartz;

namespace Shared.Extensions;

public static class QuartzExtensions
{
    public static IServiceCollection AddQuartzFromAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var jobOptions = AssembliesHelper.GetInterfaceTypes<IConfigureOptions<QuartzOptions>>(assemblies);

        foreach (var option in jobOptions) services.AddSingleton(typeof(IConfigureOptions<QuartzOptions>), option);
        return services;
    }
}