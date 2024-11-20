using Carter;
using MediatR;
using Shared.Contracts.Models;
using Shared.Contracts.Verdict;

namespace Example.Server.Weather.Features.GetWeather;

public class GetWeatherEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/weatherforecast", async (
                [AsParameters] PaginationRequest pagination,
                ISender mediator,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var query = new GetWeatherQuery(pagination);
                var result = await mediator.Send(query, cancellationToken);
                return result.Fold(onSuccess: Results.Ok, onFailure: failure => failure.ToProblemDetails(httpContext));
            })
            .WithName("GetWeatherForecast")
            .WithSummary("Get Weather Forecast");
    }
}