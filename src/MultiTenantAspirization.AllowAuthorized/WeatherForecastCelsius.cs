namespace MultiTenantAspirization.AllowAuthorized;

public class WeatherForecastCelsius
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}

public class WeatherForecastFahrenheit
{
    public DateOnly Date { get; set; }

    public int TemperatureF { get; set; }

    public string? Summary { get; set; }
}