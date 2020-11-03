using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public class TenantNotFoundException : Exception
    {
        public TenantNotFoundException() : base("Tenant Not Found")
        {
        }
    }

    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string TenantId { get; set; }
    }

    public class TenantsDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
    }

    public interface ITenantProvider
    {
        Task<Tenant> GetTenantAsync();
    }

    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly TenantsDbContext tenantsDbContext;
        private readonly ICurrentUserService currentUserService;
        private Tenant Tenant;
        private List<Tenant> Tenants;

        public TenantProvider(IHttpContextAccessor httpContextAccessor, TenantsDbContext tenantsDbContext, ICurrentUserService currentUserService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.tenantsDbContext = tenantsDbContext;
            this.currentUserService = currentUserService;
        }

        public async Task<Tenant> GetTenantAsync()
        {
            if (Tenant == null)
            {
                await SetTenant();
            }

            return Tenant;
        }

        public async Task LoadTenats()
        {
            this.Tenants = await this.tenantsDbContext.Tenants.ToListAsync();
        }

        private async Task SetTenant()
        {
            if (!Tenants.Any())
            {
                await this.LoadTenats();
            }

            if (this.TrySetTenantFromRoute() || this.TrySetTenantFromUser())
            {
                return;
            }

            throw new TenantNotFoundException();
        }

        private bool TrySetTenantFromRoute()
        {
            // Sample Code.

            return this.Tenant != null;
        }

        private bool TrySetTenantFromUser()
        {
            // Sample Code.
            this.Tenant = this.Tenants.FirstOrDefault(x => x.Id == Guid.Parse(this.currentUserService.GetUserTenantId()));
            return this.Tenant != null;
        }
    }

    public class Tenant
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ConnectionStringDb { get; set; }
    }

    public interface ICurrentUserService
    {
        string GetUserId();

        string GetUserTenantId();
    }

    public class CurrentUserService : ICurrentUserService
    {
        public string GetUserId()
        {
            return Guid.NewGuid().ToString();
        }

        public string GetUserTenantId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
