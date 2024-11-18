using Example.Server.Weather.Models;

namespace Example.Server.Weather.Features.GetWeather;

public record GetWeatherQueryHandler : IQueryHandler<GetWeatherQuery, IVerdict<List<WeatherForecast>>>
{
    public async Task<IVerdict<List<WeatherForecast>>> Handle(GetWeatherQuery request,
        CancellationToken cancellationToken)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast = Enumerable.Range(1, request.Pagination.PageSize)
            .Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                )).ToList();

        return await Task.FromResult(Verdict.Success(forecast));
    }
}