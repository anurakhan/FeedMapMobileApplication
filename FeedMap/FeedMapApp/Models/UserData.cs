using System;
using Newtonsoft.Json;
using FeedMapApp.Helpers;
namespace FeedMapApp.Models
{
    public class UserData
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; private set; }
        [JsonIgnore]
        public string Password { get; private set; }
        [JsonProperty(PropertyName = "password")]
        public string HashedPassword { get; private set; }

        public UserData(string userName, string password)
        {
            UserName = userName;
            Password = password;
            HashedPassword = EncryptUtil.SHAHashString(Password);
        }
    }
}
