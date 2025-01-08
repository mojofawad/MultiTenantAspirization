namespace MultiTenantAspirization.Web;

public class WeatherApiClient(HttpClient httpClient)
{
    public Task<WeatherForecast[]> GetPublicWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
        => GetWeatherAsync("/weatherforecast", maxItems, cancellationToken);

    public Task<WeatherForecast[]> GetAuthorizedWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
        => GetWeatherAsync("/authorized/weatherforecast", maxItems, cancellationToken);

    private async Task<WeatherForecast[]> GetWeatherAsync(string endpoint, int maxItems, CancellationToken cancellationToken)
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>(endpoint, cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}