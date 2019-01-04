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

namespace FeedMapApp.Models
{
    public class RestService
    {
        private HttpClient m_Client;
        private readonly string m_Uri = @""; //uri of the web api

        public RestService()
        {
            m_Client = new HttpClient();
        }

        public async Task<IEnumerable<FoodMarker>> GetAllFoodMarkerPosits()
        {
            string response = await m_Client.GetStringAsync(m_Uri + @"/FullFoodAndGeoData");

            if (String.IsNullOrEmpty(response)) return null;

            IEnumerable<FoodMarker> ret = JsonConvert.DeserializeObject<IEnumerable<FoodMarker>>(response);

            return ret;
        }

        public async Task<IEnumerable<FoodMarkerImageMeta>> GetFoodMarkerPhotos(int id)
        {
            HttpResponseMessage response = await m_Client.GetAsync(m_Uri + @"/Photos?foodMarkerId=" + id.ToString());
            var content = response.Content;
            var str = await content.ReadAsStringAsync();
            var ret = JsonConvert.DeserializeObject<IEnumerable<FoodMarkerImageMeta>>(str);
            foreach (var meta in ret) meta.ImageUrl = WebUtility.UrlDecode(meta.ImageUrl);
            return ret;
        }
    }

}
