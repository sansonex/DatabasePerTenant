using System.Threading.Tasks;

namespace DatabasePerTenant
{
    public interface ITenantProvider
    {
        Task<Tenant> GetTenantAsync();
    }
}
