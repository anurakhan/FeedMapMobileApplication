using System;
using FeedMapApp.Helpers;
using Foundation;
using Security;

namespace FeedMapApp.Services
{
    public class TokenPersistanceService
    {
        public TokenPersistanceService()
        {
        }

        public bool SaveToken(string token)
        {
            return KeyChainUtil.NewKeyChainRecord(WebApiCred.KeyChainTokenKey, token);
        }

        public string GetToken(string key)
        {
            return KeyChainUtil.GetKeyChainRecord(key);
        }

        public void RemoveToken(string key)
        {
            KeyChainUtil.RemoveKeyChainRecords(key);
        }
    }
}
