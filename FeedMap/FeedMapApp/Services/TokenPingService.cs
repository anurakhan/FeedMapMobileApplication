using System;
using System.Threading.Tasks;
using FeedMapApp.Models;
using FeedMapApp.Models.Abstract;

namespace FeedMapApp.Services
{
    public class TokenPingService
    {
        private RestService _restService;
        public TokenPingService()
        {
            _restService = new RestService();
            var builder = new TokenHeaderBuilder(_restService);
            builder.BuildHeaders();
        }

        public async Task<bool> IsValidToken()
        {
            var tokenService = new TokenPersistanceService();
            string token = tokenService.GetToken(WebApiCred.KeyChainTokenKey);
            if (string.IsNullOrEmpty(token))
            {
                return false;    
            }
            else
            {
                bool isValid = await _restService.IsTokenValid();
                if (!isValid)
                {
                    tokenService.RemoveToken(WebApiCred.KeyChainTokenKey);
                    return false;
                }
            }
            return true;
        }
    }
}
