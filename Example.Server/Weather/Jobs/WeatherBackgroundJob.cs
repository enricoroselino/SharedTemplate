using Microsoft.Extensions.Options;
using Quartz;

namespace Example.Server.Weather.Jobs;

public class WeatherBackgroundJob : IJob
{
    private readonly ILogger<WeatherBackgroundJob> _logger;
    private readonly TimeProvider _timeProvider;

    public WeatherBackgroundJob(ILogger<WeatherBackgroundJob> logger, TimeProvider timeProvider)
    {
        _logger = logger;
        _timeProvider = timeProvider;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Weather Background Job Handled - {_timeProvider.GetLocalNow()}");
        return Task.CompletedTask;
    }
}

public class WeatherBackgroundJobOption : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(WeatherBackgroundJob));

        options
            .AddJob<WeatherBackgroundJob>(builder => builder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger
                .ForJob(jobKey)
                .WithCronSchedule("0/20 * * * * ?"));
    }
}