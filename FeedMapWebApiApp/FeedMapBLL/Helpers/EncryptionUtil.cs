using System;
using System.Security.Cryptography;

namespace FeedMapBLL.Helpers
{
    public static class EncryptionUtil
    {
        public static byte[] GenerateHash(string password, byte[] salt, int bytesNum)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            byte[] hash = pbkdf2.GetBytes(bytesNum);
            return hash;
        }

        public static byte[] GenNewSalt(int bytesNum)
        {
            byte[] salt;
            (new RNGCryptoServiceProvider()).GetBytes(salt = new byte[bytesNum]);
            return salt;
        }
    }
}
