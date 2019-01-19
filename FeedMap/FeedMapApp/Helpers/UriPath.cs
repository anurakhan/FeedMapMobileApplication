using System;
namespace FeedMapApp.Helpers
{
    public static class UriPath
    {
        public static string Combine(string s1, string s2) => s1 + "/" + s2;
    }
}
