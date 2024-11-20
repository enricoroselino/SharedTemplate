using Example.Server.Weather.Models;
using Shared.Contracts.CQRS;
using Shared.Contracts.Models;
using Shared.Contracts.Verdict;

namespace Example.Server.Weather.Features.GetWeather;

public record GetWeatherQuery(PaginationRequest Pagination) : IQuery<IVerdict<PaginationResult<WeatherForecast>>>;