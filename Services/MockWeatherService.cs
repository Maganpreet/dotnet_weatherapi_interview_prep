using System.Diagnostics;
using WeatherApi.Services;

namespace WeatherApi.Services;

public class MockWeatherService : IWeatherService
{
    private static string[] Summaries = [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(int days = 5)
    {
        await Task.Delay(100); // Yields thread and in real it will be a DB call

        var stopwatch = Stopwatch.StartNew();
        var forecasts = Enumerable.Range(1, days).Select(index => new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            Summaries[Random.Shared.Next(Summaries.Length)]
        )).ToArray();

        stopwatch.Stop();
        Console.WriteLine($"Service generated {days} forecasts in {stopwatch.ElapsedMilliseconds}ms (async)");  // Temp: See in console

        return forecasts;
    }
}