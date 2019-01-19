using System;
namespace FeedMapWebApiApp
{
    public class ClientAuthConfigObj
    {
        public string Key { get; set; }
        public string HashBytesNum { get; set; }
        public string SaltBytesNum { get; set; }
        public string SaltHeaderKey { get; set; }
        public string ClientKeyHeader { get; set; }
        public string Token { get; set; }
    }
}
