using MultiTenantAspirization.Web;
using MultiTenantAspirization.Web.Services;

public class WeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly CurrentOrganizationService _orgService;

    public WeatherApiClient(HttpClient httpClient, CurrentOrganizationService orgService)
    {
        _httpClient = httpClient;
        _orgService = orgService;
    }

    private void AddOrgHeader(HttpRequestMessage request)
    {
        var orgId = _orgService.GetCurrentOrganizationId();
        if (orgId.HasValue)
        {
            request.Headers.Add("X-Organization-Id", orgId.Value.ToString());
        }
    }

    public async Task<WeatherForecast[]> GetPublicWeatherAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/weatherforecast");
        AddOrgHeader(request);
        
        var response = await _httpClient.SendAsync(request);
        return await response.Content.ReadFromJsonAsync<WeatherForecast[]>() ?? [];
    }

    public async Task<WeatherForecast[]> GetAuthorizedWeatherAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/authorized/weatherforecast");
        AddOrgHeader(request);
        
        var response = await _httpClient.SendAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            return Array.Empty<WeatherForecast>();
        }
        
        return await response.Content.ReadFromJsonAsync<WeatherForecast[]>() ?? [];
    }
}