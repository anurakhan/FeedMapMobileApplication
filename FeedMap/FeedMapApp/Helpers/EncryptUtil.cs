using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FeedMapApp.Helpers
{
    public static class EncryptUtil
    {
        public static byte[] HashByte(string s, byte[] salt)
        {
            return Hash(s, salt);
        }

        public static string HashString(string s, byte[] salt)
        {
            return Convert.ToBase64String(Hash(s, salt));
        }

        public static byte[] Hash(string s, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(s, salt);
            byte[] hash = pbkdf2.GetBytes(20);
            return hash;
        }

        public static byte[] GenNewSalt()
        {
            byte[] salt;
            (new RNGCryptoServiceProvider()).GetBytes(salt = new byte[16]);
            return salt;
        }

        public static string SHAHashString(string s)
        {
            return Convert.ToBase64String(SHAHash(s));
        }

        public static byte[] SHAHash(string s)
        {
            byte[] val = Encoding.UTF8.GetBytes(s).ToArray();
            return new SHA256Managed().ComputeHash(val);
        }
    }
}
