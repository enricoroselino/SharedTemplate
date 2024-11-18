namespace Shared.Infrastructure.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("[START] Handling request: {@Request}; Request Data: {@RequestData}",
            request.GetType().Name, request);

        var startTime = Stopwatch.GetTimestamp();
        var response = await next();
        var deltaSpan = Stopwatch.GetElapsedTime(startTime);

        if (deltaSpan.Seconds > 3)
            _logger.LogWarning("[PERF] Request took {@TimeTaken} seconds", deltaSpan.Seconds);

        _logger.LogInformation("[END] Handled request: {@Request}; Execution Time: {@ElapsedTime}ms",
            request.GetType().Name, deltaSpan.TotalMilliseconds);
        return response;
    }
}