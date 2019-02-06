using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FeedMapWebApiApp
{

    /// <summary>
    /// Custom Client Key Auth Middleware.
    /// NOT USED.
    /// </summary>
    public class ClientKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ClientAuthConfigObj _config;
        private TokenManagerSingleton _tokenManager;

        public ClientKeyAuthMiddleware(RequestDelegate next,
                                       IOptions<ClientAuthConfigObj> config,
                                       TokenManagerSingleton tokenManager)
        {
            _next = next;
            _config = config.Value;
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var headerDict = context.Request.Headers;

            if (IsValid(headerDict))
                await _next(context);
            else
            {
                context.Response.StatusCode = 401; //UnAuthorized                
                await context.Response.WriteAsync("UnAuthorized");
                return;
            }

        }

        private bool IsValid(IHeaderDictionary headerDictionary)
        {
            if (IsTokenAuth(headerDictionary)) return true;

            string clientKey = 
                headerDictionary[_config.ClientKeyHeader];
            if (String.IsNullOrWhiteSpace(clientKey)) return false;

            string clientSalt =
                headerDictionary[_config.SaltHeaderKey];
            if (String.IsNullOrWhiteSpace(clientSalt)) return false;

            return IsSame(clientKey, clientSalt,
                          _config.Key,
                          Convert.ToInt32(_config.HashBytesNum));
        }

        private bool IsTokenAuth(IHeaderDictionary headerDictionary)
        {
            string token = headerDictionary[_config.Token];
            if (String.IsNullOrWhiteSpace(token)) return false;

            int id = -1;
            return _tokenManager.HasTokenInPool(token, ref id);
        }

        private bool IsSame(string key, 
                           string salt, 
                           string webApiKey, 
                           int bytesNum)
        {
            var byteSalt = Convert.FromBase64String(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(webApiKey, byteSalt);
            byte[] hash = pbkdf2.GetBytes(bytesNum);
                       
            return (key == Convert.ToBase64String(hash));
        }
    }
}
