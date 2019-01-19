using System;
using Newtonsoft.Json;

namespace FeedMapWebApiApp.Models
{
    public class UserDataClient
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string HashedPassword { get; set; }
    }
}
