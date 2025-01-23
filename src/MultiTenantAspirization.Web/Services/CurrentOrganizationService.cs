namespace MultiTenantAspirization.Web.Services;

public class CurrentOrganizationService
{
    private AsyncLocal<Guid?> _currentOrganizationId = new();
    
    public Guid? GetCurrentOrganizationId()
    {
        return _currentOrganizationId.Value;
    }
    
    public void SetCurrentOrganizationId(Guid? organizationId)
    {
        _currentOrganizationId.Value = organizationId;
    }
}