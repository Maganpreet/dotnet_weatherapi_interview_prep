using System.Diagnostics;

namespace WeatherApi.Middleware;

public class RequestTracingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTracingMiddleware(RequestDelegate next)
    {
        _next = next; // Constructor DI
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var logger = context.RequestServices.GetRequiredService<ILogger<RequestTracingMiddleware>>();
        logger.LogInformation("Request incoming: {Method} {Path} at {Time}",
            context.Request.Method, context.Request.Path, DateTime.UtcNow);

        await _next(context);

        // Post-request: Log response details (runs after endpoint/controller)
        stopwatch.Stop();
        logger.LogInformation("Request completed: {Method} {Path} in {ElapsedMs}ms, Status: {Status}",
            context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds, context.Response.StatusCode);
    }
}