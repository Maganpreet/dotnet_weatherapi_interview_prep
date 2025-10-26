namespace WeatherApi.Services;

public interface IWeatherService
{
    Task<IEnumerable<WeatherForecast>> GetForecastAsync(int days = 5);
}