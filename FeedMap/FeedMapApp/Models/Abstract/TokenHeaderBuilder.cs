using System;
using FeedMapApp.Services;

namespace FeedMapApp.Models.Abstract
{
    public class TokenHeaderBuilder : IHeaderBuilder
    {
        private RestService _restService;

        public TokenHeaderBuilder(RestService restService)
        {
            _restService = restService;
        }

        public void BuildHeaders()
        {
            TokenPersistanceService tokenPersistanceSerivce = new TokenPersistanceService();
            string token = tokenPersistanceSerivce.GetToken(WebApiCred.KeyChainTokenKey);
            _restService.AddHttpHeader("tokenAuth", 
                                       token);
        }
    }
}
