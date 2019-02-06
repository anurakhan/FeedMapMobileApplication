using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FeedMapApp.Models;

namespace FeedMapApp.Services
{
    public class ExternalRestService
    {
        private HttpClient m_Client;
        private readonly string _key = "f59c4ca0a2c99ac15a555999904189148c8ca0a";

        public ExternalRestService()
        {
            m_Client = new HttpClient();
        }

        public async Task<RestReturnObj<string>> GetRestaurantAddressGeocode(double lat, double lng)
        {
            string uri = @"https://api.geocod.io/v1.3/reverse?q="
                + lat.ToString() + "," + lng.ToString() + "&api_key=" + _key;

            HttpResponseMessage response = await m_Client.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
                return new RestReturnObj<string> { IsSuccess = false };

            var content = response.Content;
            var str = await content.ReadAsStringAsync();
            Match match = Regex.Match(str, "\"formatted_address\":\"(.+?)\",");
            string address = match.Groups[1].Value;
            return new RestReturnObj<string> { IsSuccess = true,
                Obj = address};
        }
    }
}
