namespace MultiTenantAspirization.AllowAuthorized.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<OrganizationMember> Organizations { get; set; }
}