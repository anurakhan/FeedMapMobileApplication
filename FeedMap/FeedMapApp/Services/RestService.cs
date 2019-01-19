using Foundation;
using System;
using System.IO;
using UIKit;
using MapKit;
using CoreGraphics;
using CoreLocation;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System.Text;
using FeedMapApp.Helpers;

namespace FeedMapApp.Models
{
    public class RestService
    {
        private HttpClient m_Client;
        private readonly string m_Uri = WebApiCred.WebApiUri; //uri of the web api

        public RestService()
        {
            m_Client = new HttpClient();
        }

        public void AddHttpHeader(string key, string value)
        {
            m_Client.DefaultRequestHeaders.Add(key, value);
        }

        public async Task<RestReturnObj<IEnumerable<FoodMarker>>> GetAllFoodMarkerPosits()
        {
            HttpResponseMessage responseMsg = await m_Client.GetAsync(m_Uri + @"/FullFoodAndGeoData");

            if (!responseMsg.IsSuccessStatusCode) 
                return new RestReturnObj<IEnumerable<FoodMarker>> { IsSuccess = false };

            var response = await responseMsg.Content.ReadAsStringAsync();

            if (String.IsNullOrEmpty(response)) return null;

            IEnumerable<FoodMarker> ret = JsonConvert.DeserializeObject<IEnumerable<FoodMarker>>(response);

            return new RestReturnObj<IEnumerable<FoodMarker>>
            {
                IsSuccess = responseMsg.IsSuccessStatusCode,
                Obj = ret
            };
        }

        public async Task<RestReturnObj<IEnumerable<FoodMarkerImageMeta>>> GetFoodMarkerPhotos(int id)
        {
            HttpResponseMessage response = await m_Client.GetAsync(m_Uri + @"/Photos?foodMarkerId=" + id.ToString());

            if (!response.IsSuccessStatusCode)
                return new RestReturnObj<IEnumerable<FoodMarkerImageMeta>> { IsSuccess = false };

            var content = response.Content;
            var str = await content.ReadAsStringAsync();
            var ret = JsonConvert.DeserializeObject<IEnumerable<FoodMarkerImageMeta>>(str);
            foreach (var meta in ret) meta.ImageUrl = WebUtility.UrlDecode(meta.ImageUrl);
            return new RestReturnObj<IEnumerable<FoodMarkerImageMeta>>
            {
                IsSuccess = response.IsSuccessStatusCode,
                Obj = ret
            };
        }


        public async Task<RestReturnObj<UserToken>> UserAuthRequest(string apiResources, UserData userData)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(userData), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await m_Client.PostAsync(UriPath.Combine(m_Uri, apiResources), content);

            if (!response.IsSuccessStatusCode)
                return new RestReturnObj<UserToken> { IsSuccess = false };

            var str = await response.Content.ReadAsStringAsync();
            UserToken token = new UserToken();
            if (!String.IsNullOrWhiteSpace(str))
            {
                token = JsonConvert.DeserializeObject<UserToken>(str);
            }
            return new RestReturnObj<UserToken>{ IsSuccess = response.IsSuccessStatusCode,
                Obj = token};
        }

        public async Task<bool> IsTokenValid()
        {
            HttpContent content = new StringContent("");
            HttpResponseMessage response = await m_Client.PostAsync(UriPath.Combine(m_Uri, "Ping"), content);
            return response.IsSuccessStatusCode;
        }
    }

}
