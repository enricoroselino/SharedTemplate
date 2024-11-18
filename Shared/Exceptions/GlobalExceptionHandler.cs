using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Shared.Exceptions;

internal class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    // ReSharper disable once TooManyDeclarations
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            "[ERR] Message: {@ExceptionMessage}, Time of occurrence {@Time}",
            exception.Message, DateTime.UtcNow);

        (string Type, string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException =>
            (
                "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                "Something went wrong",
                "Internal Server Error",
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
            ),
            ValidationException =>
            (
                "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                exception.Message,
                "one or more validation errors occurred.",
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            BadRequestException =>
            (
                "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                exception.Message,
                "Bad Request",
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            NotFoundException =>
            (
                "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                exception.Message,
                "Not Found",
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound
            ),
            _ =>
            (
                "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                "Something went wrong",
                "Unknown Error",
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
            )
        };

        var problemDetails = new ProblemDetails
        {
            Type = details.Type,
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions.Add("TraceId", httpContext.TraceIdentifier);

        if (exception is ValidationException validationExceptions)
        {
            var errorsDictionary = validationExceptions.Errors
                .Where(x => x is not null)
                .GroupBy(
                    x => x.PropertyName,
                    x => x.ErrorMessage,
                    (propertyName, errorsMessages) =>
                        new { Key = propertyName, Messages = errorsMessages.Distinct().ToArray() })
                .ToDictionary(x => x.Key, x => x.Messages);

            problemDetails.Extensions.Add("ValidationErrors", errorsDictionary);
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;
    }
}