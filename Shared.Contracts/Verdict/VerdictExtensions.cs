using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Contracts.Verdict;

public static class VerdictExtensions
{
    public static IResult ToProblemDetails(this IFailure f, HttpContext httpContext)
    {
        var problemDetails = new ProblemDetails
        {
            Status = FailureHelper.GetStatusCode(f.Type),
            Title = FailureHelper.GetTitle(f.Type),
            Detail = f.Message,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions.Add("TraceId", httpContext.TraceIdentifier);
        return Results.Problem(problemDetails);
    }
}