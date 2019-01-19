using System;
using System.Security.Cryptography;
using FeedMapBLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace FeedMapWebApiApp
{
    public class ClientKeyAuthorizeFilter : IAuthorizationFilter
    {
        private readonly ClientAuthConfigObj _config;
        private TokenManagerSingleton _tokenManager;

        public ClientKeyAuthorizeFilter(
                                       IOptions<ClientAuthConfigObj> config,
                                       TokenManagerSingleton tokenManager)
        {
            _config = config.Value;
            _tokenManager = tokenManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headerDict = context.HttpContext.Request.Headers;
            if (!IsValid(headerDict))
            {
                context.Result = new UnauthorizedResult();
            }
        }


        private bool IsValid(IHeaderDictionary headerDictionary)
        {
            string clientKey =
                headerDictionary[_config.ClientKeyHeader];
            if (String.IsNullOrWhiteSpace(clientKey)) return false;

            string clientSalt =
                headerDictionary[_config.SaltHeaderKey];
            if (String.IsNullOrWhiteSpace(clientSalt)) return false;

            var encryptionService = new EncryptionService(clientKey,
                                                          clientSalt,
                                                          Convert.ToInt32(_config.HashBytesNum));

            return (encryptionService.Compare(_config.Key) 
                    == EncryptionService.EncryptComp.Equal);
        }
    }
}
