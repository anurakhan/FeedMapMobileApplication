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

            RetObj<IEnumerable<FoodMarker>> ret = JsonConvert.DeserializeObject<RetObj<IEnumerable<FoodMarker>>>(response);

            if (ret.IsSuccess)
            {
                return ret.ResponseObj;
            }
            else
            {
                return null;
            }
        }

        public async Task<Stream> GetFoodMarkerPhotos(int id)
        {
            HttpResponseMessage response = await m_Client.GetAsync(m_Uri + @"/Photos/" + id.ToString());
            return await response.Content.ReadAsStreamAsync();
        }
    }

    public class RetObj<T>
    {
        public T ResponseObj { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

}
