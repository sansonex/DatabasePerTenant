using Microsoft.EntityFrameworkCore;

namespace DatabasePerTenant
{
    public class TenantsDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
    }
}
