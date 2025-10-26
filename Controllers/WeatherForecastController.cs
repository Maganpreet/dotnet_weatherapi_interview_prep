using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
    {
        _logger.LogInformation($"Request received at : {DateTime.Now}");

        try
        {
            var forecast = await _weatherService.GetForecastAsync(); // Await non blocking shit
            _logger.LogInformation("Controller: Forecast retrieved successfully");

            return Ok(forecast);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Controller: Error fetching forecast");
            return StatusCode(500, "Internal server error");  // Preview exception handling
        }
    }
}