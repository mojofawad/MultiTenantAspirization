using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MultiTenantAspirization.Web;
using MultiTenantAspirization.Web.Components;
using MultiTenantAspirization.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

var oidcScheme = OpenIdConnectDefaults.AuthenticationScheme;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddKeycloakOpenIdConnect("keycloak", realm: "aspirization-realm", oidcScheme, options =>
    {
        // Use configuration from appsettings.json
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.ClientId = builder.Configuration["Keycloak:ClientId"];
        options.ClientSecret = builder.Configuration["Keycloak:ClientSecret"];
    
        options.CallbackPath = "/signin-oidc";
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.Scope.Add("weather:all");
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
        options.SaveTokens = true;
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    
        // Add debugging events
        options.Events = new OpenIdConnectEvents
        {
            OnRemoteFailure = context =>
            {
                var failure = context.Failure;
                Console.WriteLine($"Remote failure: {failure?.Message}");
                return Task.CompletedTask;
            },
            OnRedirectToIdentityProvider = context =>
            {
                Console.WriteLine($"Redirecting to identity provider: {context.ProtocolMessage.IssuerAddress}");
                return Task.CompletedTask;
            }
        };
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthenticationCore();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://apigateway");
    });

builder.Services.AddScoped<CurrentOrganizationService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.MapDefaultEndpoints();
app.MapLoginAndLogout();

app.Run();
