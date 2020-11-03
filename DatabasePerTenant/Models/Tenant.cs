using System;

namespace DatabasePerTenant
{
    public class Tenant
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ConnectionStringDb { get; set; }
    }
}
