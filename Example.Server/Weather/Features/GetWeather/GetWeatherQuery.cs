using Example.Server.Weather.Models;
using Shared.Contracts.Models;

namespace Example.Server.Weather.Features.GetWeather;

public record GetWeatherQuery(PaginationRequest Pagination) : IQuery<IVerdict<PaginationResult<WeatherForecast>>>;