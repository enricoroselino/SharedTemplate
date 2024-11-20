using Example.Server.Weather.Models;
using Shared.Contracts.CQRS;
using Shared.Contracts.Models;
using Shared.Contracts.Verdict;

namespace Example.Server.Weather.Features.GetWeather;

public record GetWeatherQueryHandler : IQueryHandler<GetWeatherQuery, IVerdict<PaginationResult<WeatherForecast>>>
{
    public async Task<IVerdict<PaginationResult<WeatherForecast>>> Handle(
        GetWeatherQuery request,
        CancellationToken cancellationToken)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var pageIndex = request.Pagination.PageIndex;
        var pageSize = request.Pagination.PageSize;

        var forecast = Enumerable.Range(1, pageSize)
            .Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToList();

        var result = new PaginationResult<WeatherForecast>(pageIndex, pageSize, forecast.Count, forecast);
        return Verdict.Success(result);
    }
}