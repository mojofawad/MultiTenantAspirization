using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MultiTenantAspirization.AllowAuthorized.Policies.Requirements;

namespace MultiTenantAspirization.AllowAuthorized.Policies.Handlers;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly AuthDbContext _dbContext;
    private readonly HttpContextAccessor _httpContextAccessor;
    
    public PermissionHandler(AuthDbContext dbContext, HttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // userId int
        var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        var organizationId = int.Parse(_httpContextAccessor.HttpContext?.Request.Headers["OrganizationId"]);

        var hasPermission = await _dbContext.Permissions
            .Include(p => p.OrganizationMember)
            .ThenInclude(om => om.User)
            .AnyAsync(p => p.OrganizationMember.User.Id == userId &&
                p.OrganizationMember.OrganizationId == organizationId &&
                p.Name == requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}