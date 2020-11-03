namespace DatabasePerTenant
{
    public interface ICurrentUserService
    {
        string GetUserId();

        string GetUserTenantId();
    }
}
