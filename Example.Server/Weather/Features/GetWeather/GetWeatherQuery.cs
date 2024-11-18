using Example.Server.Weather.Models;

namespace Example.Server.Weather.Features.GetWeather;

public record GetWeatherQuery(PaginationRequest Pagination) : IQuery<IVerdict<List<WeatherForecast>>>;