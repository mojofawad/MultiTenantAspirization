namespace MultiTenantAspirization.AllowAuthorized.Entities;

public class OrganizationMember
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<Permission> Permissions { get; set; }
}