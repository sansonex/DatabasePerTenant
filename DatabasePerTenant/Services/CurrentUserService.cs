using System;

namespace DatabasePerTenant
{
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
