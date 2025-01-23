using Microsoft.AspNetCore.Authorization;

namespace MultiTenantAspirization.AllowAuthorized.Policies.Requirements;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }
    
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
    
}