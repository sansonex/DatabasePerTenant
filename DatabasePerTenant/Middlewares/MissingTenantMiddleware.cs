using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabasePerTenant.Midlewares
{
    /// <summary>
    /// Missing Tenant Middleware class.
    /// </summary>
    public class MissingTenantMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingTenantMiddleware"/> class.
        /// Missing Tenant constructor.
        /// </summary>
        /// <param name="next">Request Delegate.</param>
        public MissingTenantMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Invoke method.
        /// </summary>
        /// <param name="httpContext">http context.</param>
        /// <param name="provider">provider.</param>
        /// <returns>nothing at all.</returns>
        public async Task Invoke(HttpContext httpContext, ITenantProvider provider)
        {
            if (await provider.GetTenantAsync() == null)
            {
                throw new TenantNotFoundException();
            }

            await this.next.Invoke(httpContext);
        }
    }
}
