namespace MultiTenantAspirization.AllowAuthorized.Entities;

public class Permission
{
    public int Id { get; set; }
    public int OrganizationMemberId { get; set; }
    public OrganizationMember OrganizationMember { get; set; }
    public string Name { get; set; }
}