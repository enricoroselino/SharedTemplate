using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Infrastructure.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly TimeProvider _timeProvider;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, TimeProvider timeProvider)
    {
        _logger = logger;
        _timeProvider = timeProvider;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("[START] Handling request: {@Request}; Request Data: {@RequestData}",
            request.GetType().Name, request);

        var startTime = _timeProvider.GetTimestamp();
        var response = await next();
        var deltaSpan = _timeProvider.GetElapsedTime(startTime);

        if (deltaSpan.Seconds > 3)
            _logger.LogWarning("[PERF] Request took {@TimeTaken} seconds", deltaSpan.TotalSeconds);

        _logger.LogInformation("[END] Handled request: {@Request}; Execution Time: {@ElapsedTime}ms",
            request.GetType().Name, deltaSpan.TotalMilliseconds);
        return response;
    }
}