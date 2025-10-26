using Microsoft.Extensions.Caching.Memory;

namespace WeatherApi.Services;

public class CachedWeatherService : IWeatherService
{
    private readonly IWeatherService _innerService;
    private readonly IMemoryCache _cache;

    public CachedWeatherService(IWeatherService innerService, IMemoryCache cache)
    {
        _innerService = innerService;
        _cache = cache;
    }

    public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(int days = 5)
    {
        var cacheKey = $"forecast_{days}_{DateTime.Today:yyyy-MM-dd}"; // Daily cache
        if (_cache.TryGetValue(cacheKey, out IEnumerable<WeatherForecast>? cachedForecasts))
        {
            Console.WriteLine("Service: Returning cached forecast");  // Temp log
            return cachedForecasts!;
        }

        var forecasts = await _innerService.GetForecastAsync(days);
        _cache.Set(cacheKey, forecasts, TimeSpan.FromMinutes(15));

        Console.WriteLine("Service: Generated and cached new forecast");
        return forecasts;
    }
}