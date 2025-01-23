namespace MultiTenantAspirization.AllowAuthorized.Entities;

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<OrganizationMember> Members { get; set; }
}