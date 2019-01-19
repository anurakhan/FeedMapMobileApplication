using System;
using FeedMapBLL.Helpers;

namespace FeedMapBLL.Services
{
    public class EncryptionService
    {
        public enum EncryptComp
        {
            Equal,
            UnEqual
        };

        private string _key;
        private string _salt;
        private int _byteNum;

        public EncryptionService(string key, string salt, int byteNum)
        {
            _key = key;
            _salt = salt;
            _byteNum = byteNum;
        }

        public EncryptComp Compare(string cmpKey)
        {
            var byteSalt = Convert.FromBase64String(_salt);

            var hashedCmpKey = EncryptionUtil.GenerateHash(cmpKey,
                                                           byteSalt,
                                                           _byteNum);

            return (_key == Convert.ToBase64String(hashedCmpKey) ?
                    EncryptComp.Equal : EncryptComp.UnEqual);           
        }
    }
}
