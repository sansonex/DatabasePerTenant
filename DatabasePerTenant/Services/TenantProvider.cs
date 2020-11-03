using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabasePerTenant
{
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
}
