var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder.AddKeycloak("keycloak", 8080);

var allowAllApi = builder.AddProject<Projects.MultiTenantAspirization_AllowAll>("allow-all-api")
    .WithReference(keycloak);

var allowAuthorizedApi = builder.AddProject<Projects.MultiTenantAspirization_AllowAuthorized>("allow-authorized-api")
    .WithReference(keycloak);

var apiGateway = builder.AddProject<Projects.MultiTenantAspirization_Gateway>("apigateway")
    .WithReference(allowAllApi)
    .WithReference(allowAuthorizedApi);

builder.AddProject<Projects.MultiTenantAspirization_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiGateway)
    .WaitFor(apiGateway);

builder.Build().Run();
