using System;
using FeedMapBLL.Helpers;

namespace FeedMapBLL.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public string PasswordRaw { get; private set;}

        public User()
        {
            
        }

        public User(string userName, string password)
        {
            UserName = userName;
            PasswordRaw = password;
        }

        public User(int id, string userName, 
                   byte[] passwordHash, byte[] passwordSalt)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public void GenPasswordHash(byte[] salt, int byteNum)
        {
            PasswordSalt = salt;
            byte[] hash = EncryptionUtil.GenerateHash(PasswordRaw, salt,
                                                      byteNum);
            PasswordHash = hash;
        }

        public bool DatawiseEquals(User user)
        {
            return ((this.UserName == user.UserName) &&
             (Convert.ToBase64String(this.PasswordHash) == Convert.ToBase64String(user.PasswordHash)));
        }
    }
}
