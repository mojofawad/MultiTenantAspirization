var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "admin");
var password = builder.AddParameter("password", "admin", secret: true);

var postgres = builder.AddPostgres("postgres");
var authDb = postgres.AddDatabase("authdb");

var keycloak = builder.AddKeycloak("keycloak", 8080, username, password)
    .WithDataVolume()
    .WithRealmImport("./Realms")
    .WithEnvironment("KC_BOOTSTRAP_ADMIN_USERNAME", username)
    .WithEnvironment("KC_BOOTSTRAP_ADMIN_PASSWORD", password);

var authServer = builder.AddProject<Projects.MultiTenantAspirization_Authorization>("authserver")
    .WithReference(authDb)
    .WaitFor(authDb);

var allowAllApi = builder.AddProject<Projects.MultiTenantAspirization_AllowAll>("allow-all-api");
var allowAuthorizedApi = builder.AddProject<Projects.MultiTenantAspirization_AllowAuthorized>("allow-authorized-api");

var apiGateway = builder.AddProject<Projects.MultiTenantAspirization_Gateway>("apigateway")
    .WithReference(keycloak)
    .WithReference(authServer)
    .WithReference(allowAllApi)
    .WithReference(allowAuthorizedApi);

builder.AddProject<Projects.MultiTenantAspirization_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(keycloak)
    .WithReference(apiGateway)
    .WaitFor(apiGateway);

builder.Build().Run();
