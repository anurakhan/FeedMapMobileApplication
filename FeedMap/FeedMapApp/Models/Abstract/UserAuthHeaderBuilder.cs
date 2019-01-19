using System;
using FeedMapApp.Helpers;

namespace FeedMapApp.Models.Abstract
{
    public class UserAuthHeaderBuilder : IHeaderBuilder
    {
        private RestService _restService;

        public UserAuthHeaderBuilder(RestService restService)
        {
            _restService = restService;
        }

        public void BuildHeaders()
        {
            var clientKeySecretFac = EncryptUtil.GenNewSalt();
            _restService.AddHttpHeader("clientkeySecretFac", Convert.ToBase64String(clientKeySecretFac));
            _restService.AddHttpHeader("clientkey", EncryptUtil.HashString(WebApiCred.AuthClientKey,
                                                                                   clientKeySecretFac));
        }
    }
}
