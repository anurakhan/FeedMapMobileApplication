using System;
using Microsoft.AspNetCore.Builder;

namespace FeedMapWebApiApp
{
    /// <summary>
    /// Extention method for application builder that hooks up custom client key auth middleware.
    /// NOT USED.
    /// </summary>
    public static class ClientKeyAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseClientKeyAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClientKeyAuthMiddleware>();
        }
    }
}
