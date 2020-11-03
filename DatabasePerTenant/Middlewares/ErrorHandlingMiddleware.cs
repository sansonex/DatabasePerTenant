using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DatabasePerTenant.Midlewares
{
    /// <summary>
    /// Error Handling middleware class.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="next">The next request delegate.</param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        ///     Asynchronous Invoke.
        /// </summary>
        /// <param name="context">The HttpContext.</param>
        /// <returns>The asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception exception)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string responseBody;

            context.Response.Clear();

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            responseBody = exception.Message;

            await context.Response.WriteAsync(responseBody);
        }
    }
}
