using Microsoft.Extensions.Caching.Memory;
using WeatherApi.Filters;
using WeatherApi.Middleware;
using WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
builder.Services.AddMemoryCache();

builder.Services.AddScoped<MockWeatherService>(); // Scoped Fresh instance per request
builder.Services.AddScoped<IWeatherService>(provider =>
{
    var innerService = provider.GetRequiredService<MockWeatherService>();
    var cache = provider.GetRequiredService<IMemoryCache>();
    return new CachedWeatherService(innerService, cache);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseRequestTracing();
app.MapControllers();

app.Run();
