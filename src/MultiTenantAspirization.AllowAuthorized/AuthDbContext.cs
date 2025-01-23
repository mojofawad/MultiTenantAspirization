using Microsoft.EntityFrameworkCore;
using MultiTenantAspirization.AllowAuthorized.Entities;

namespace MultiTenantAspirization.AllowAuthorized;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
    
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationMember> OrganizationMembers { get; set; }
    public DbSet<User> Users { get; set; }
}