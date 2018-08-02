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

        public async Task<Stream> GetFoodMarkerPhotos(int id)
        {
            HttpResponseMessage response = await m_Client.GetAsync(m_Uri + @"/Photos?foodMarkerId=" + id.ToString());
            return await response.Content.ReadAsStreamAsync();
        }
    }

}
