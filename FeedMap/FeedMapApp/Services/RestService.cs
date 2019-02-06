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

            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                return new RestReturnObj<IEnumerable<FoodMarkerImageMeta>> { IsSuccess = false };

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new RestReturnObj<IEnumerable<FoodMarkerImageMeta>>
                {
                    IsSuccess = true,
                    Obj = new List<FoodMarkerImageMeta>()
                };
            }
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

        public async Task<RestReturnObj<IEnumerable<FoodCategories>>> GetAllFoodCategories()
        {
            HttpResponseMessage responseMsg = await m_Client.GetAsync(UriPath.Combine(m_Uri, "FoodCategories"));

            if (!responseMsg.IsSuccessStatusCode)
                return new RestReturnObj<IEnumerable<FoodCategories>> { IsSuccess = false };

            var response = await responseMsg.Content.ReadAsStringAsync();

            if (String.IsNullOrEmpty(response)) return null;

            IEnumerable<FoodCategories> ret = 
                JsonConvert.DeserializeObject<IEnumerable<FoodCategories>>(response);

            return new RestReturnObj<IEnumerable<FoodCategories>>
            {
                IsSuccess = responseMsg.IsSuccessStatusCode,
                Obj = ret
            };
        }

        public async Task<RestReturnObj<PostedFoodMarkerRetObj>> PostFoodMarker(FoodMarker foodMarker)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(foodMarker), Encoding.UTF8,
                                                    "application/json");
            HttpResponseMessage responseMsg = await m_Client.PostAsync(UriPath.Combine(m_Uri, "FullFoodAndGeoData"),
                                                                      content);

            if (!responseMsg.IsSuccessStatusCode)
                return new RestReturnObj<PostedFoodMarkerRetObj>(){IsSuccess = false};
            
            var response = await responseMsg.Content.ReadAsStringAsync();

            if (String.IsNullOrEmpty(response)) return null;

            PostedFoodMarkerRetObj ret =
                JsonConvert.DeserializeObject<PostedFoodMarkerRetObj>(response);

            return new RestReturnObj<PostedFoodMarkerRetObj>
            {
                IsSuccess = responseMsg.IsSuccessStatusCode,
                Obj = ret
            };
        }

        public async Task<RestReturnObj<IEnumerable<FoodMarkerImageMeta>>> PostPhotos(int foodMarkerId, List<byte[]> images)
        {
            string uri = UriPath.Combine(m_Uri, "Photos");
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, UriPath.Combine(uri, foodMarkerId.ToString()));

            MultipartFormDataContent content = new MultipartFormDataContent();
            foreach (var image in images)
            {
                ByteArrayContent byteArrayContent = new ByteArrayContent(image);
                byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");
                Guid uniqueId = Guid.NewGuid(); 
                content.Add(byteArrayContent, "file", "image" + uniqueId.ToString() + ".png");
            }

            requestMessage.Content = content;
            
            HttpResponseMessage response = await m_Client.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
                return new RestReturnObj<IEnumerable<FoodMarkerImageMeta>>{IsSuccess = false};

            var resContent = response.Content;
            var str = await resContent.ReadAsStringAsync();
            var ret = JsonConvert.DeserializeObject<IEnumerable<FoodMarkerImageMeta>>(str);
            foreach (var meta in ret) meta.ImageUrl = WebUtility.UrlDecode(meta.ImageUrl);
            return new RestReturnObj<IEnumerable<FoodMarkerImageMeta>>
            {
                IsSuccess = response.IsSuccessStatusCode,
                Obj = ret
            }; 
        }

        public async Task<bool> DeleteFoodMarker(int foodMarkerId)
        {
            string uri = UriPath.Combine(m_Uri, "FoodMarker");
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, UriPath.Combine(uri, foodMarkerId.ToString()));

            HttpResponseMessage response = await m_Client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }


        public async Task<bool> DeletePhotos(int foodMarkerId)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                                               m_Uri + @"/Photos?foodMarkerId=" + foodMarkerId.ToString());


            HttpResponseMessage response = await m_Client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
    }

}
