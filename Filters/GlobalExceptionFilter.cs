using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WeatherApi.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        _logger.LogError(exception, "Global handler caught unhandled exception: {Message}", exception.Message);

        // Standardized response: No stack trace in prod
        context.Result = new ObjectResult(
            new
            {
                Error = "An unexpected error occurred",
                Message = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment()
                            ? exception.Message
                            : "Please try again later"
            })
        {
            StatusCode = 500
        };

        context.ExceptionHandled = true; // Stops bubbling
    }
}