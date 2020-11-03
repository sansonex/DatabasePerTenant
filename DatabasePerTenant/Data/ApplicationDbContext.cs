using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DatabasePerTenant
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantProvider tenantProvider;

        public ApplicationDbContext(ITenantProvider tenantProvider)
        {
            this.tenantProvider = tenantProvider;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenant = this.tenantProvider.GetTenantAsync().GetAwaiter().GetResult();

            //if (tenant != null)
            //{
            //    optionsBuilder.UseSqlServer(tenant.ConnectionStringDb);
            //}

            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            base.OnConfiguring(optionsBuilder);
        }
    }
}
