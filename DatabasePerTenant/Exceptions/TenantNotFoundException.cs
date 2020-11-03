using System;

namespace DatabasePerTenant
{
    public class TenantNotFoundException : Exception
    {
        public TenantNotFoundException() : base("Tenant Not Found")
        {
        }
    }
}
