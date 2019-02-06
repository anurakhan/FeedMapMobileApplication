using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace FeedMapWebApiApp
{
    public class TokenAuthorizeFilter : IAuthorizationFilter
    {
        private readonly ClientAuthConfigObj _config;
        private TokenManagerSingleton _tokenManager;
        private int _id = -1;


        public TokenAuthorizeFilter(IOptions<ClientAuthConfigObj> config,
                                    TokenManagerSingleton tokenManager)
        {
            _config = config.Value;
            _tokenManager = tokenManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.FilterDescriptors.Any(filter =>
            filter.Filter is ClientKeyAuthAttribute))
            {
                return;
            }

            var headerDict = context.HttpContext.Request.Headers;
            if (!IsTokenAuth(headerDict))
            {
                context.Result = new UnauthorizedResult();
            }

            context.RouteData.Values.Add("UserId", _id);
        }

        private bool IsTokenAuth(IHeaderDictionary headerDictionary)
        {
            string token = headerDictionary[_config.Token];
            if (String.IsNullOrWhiteSpace(token)) return false;

            return _tokenManager.HasTokenInPool(token, ref _id);
        }

    }
}
