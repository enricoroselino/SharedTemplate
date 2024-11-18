using Microsoft.AspNetCore.Http;

namespace Shared.Contracts.Verdict;

internal static class FailureHelper
{
    public static int GetStatusCode(FailureType errorType) =>
        errorType switch
        {
            FailureType.BadRequest => StatusCodes.Status400BadRequest,
            FailureType.Conflict => StatusCodes.Status409Conflict,
            FailureType.NotFound => StatusCodes.Status404NotFound,
            FailureType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

    public static string GetTitle(FailureType errorType) =>
        errorType switch
        {
            FailureType.BadRequest => "Bad Request",
            FailureType.Conflict => "Conflict",
            FailureType.NotFound => "Not Found",
            FailureType.Forbidden => "Forbidden",
            FailureType.Server => "Internal Server Error",
            _ => "Unknown Error"
        };
}