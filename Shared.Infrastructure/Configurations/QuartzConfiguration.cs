namespace Shared.Infrastructure.Configurations;

public static class QuartzConfiguration
{
    public static IServiceCollection AddQuartzConfiguration(this IServiceCollection services)
    {
        services.AddQuartz();
        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = false; });
        return services;
    }
}