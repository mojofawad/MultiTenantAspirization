using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MultiTenantAspirization.AllowAuthorized.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger = logger;

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecastCelsius> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecastCelsius
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [Authorize(Policy = "weather:read-celsius")]
    [HttpGet(Name = "GetWeatherCelsius")]
    public IEnumerable<WeatherForecastCelsius> GetCelsius()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecastCelsius
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [Authorize(Policy = "weather:read-fahrenheit")]
    [HttpGet(Name = "GetWeatherFahrenheit")]
    public IEnumerable<WeatherForecastFahrenheit> GetFahrenheit()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecastFahrenheit
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureF = 32 + (int)(Random.Shared.Next(-20, 55) / 0.5556),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}