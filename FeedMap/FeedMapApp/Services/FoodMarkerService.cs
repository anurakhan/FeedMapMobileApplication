using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FeedMapApp.Models;
using FeedMapApp.Models.Abstract;

namespace FeedMapApp.Services
{
    public class FoodMarkerService
    {
        private RestService _restService;
        private ExternalRestService _extRestService;
        private TokenPersistanceService _tokenPersistance;
        public event EventHandler OnFail;

        public FoodMarkerService()
        {
            _tokenPersistance = new TokenPersistanceService();
            _restService = new RestService();
            _extRestService = new ExternalRestService();
            TokenHeaderBuilder headerBuilder = new TokenHeaderBuilder(_restService);
            headerBuilder.BuildHeaders();
        }

        public async Task<IEnumerable<FoodMarker>> GetAllFoodMarkerPositions()
        {
            var ret = await _restService.GetAllFoodMarkerPosits();
            if (!ret.IsSuccess)
            {
                _tokenPersistance.RemoveToken(WebApiCred.KeyChainTokenKey);
                OnFail(ret, new EventArgs());
            }
            return ret.Obj;
        }

        public async Task<IEnumerable<FoodMarkerImageMeta>> GetFoodMarkerPhotos(int id)
        {
            var ret = await _restService.GetFoodMarkerPhotos(id);
            if (!ret.IsSuccess)
            {
                _tokenPersistance.RemoveToken(WebApiCred.KeyChainTokenKey);
                OnFail(ret, new EventArgs());
            }
            return ret.Obj;
        }

        public async Task<bool> SaveFoodMarker(FoodMarker foodMarker,
                                              double lat,
                                              double lng)
        {
            var addressRet = await _extRestService.GetRestaurantAddressGeocode(lat, lng);
            if (!addressRet.IsSuccess) return false;
            string address = addressRet.Obj;

            foodMarker.RestaurantAddress = address;
            foodMarker.RestaurantPosition = $"POINT({lng} {lat})";

            var _validator = new FoodMarkerValidator(foodMarker);
            if (!_validator.Validate()) return false;

            var ret = await _restService.PostFoodMarker(foodMarker);
            if (!ret.IsSuccess)
            {
                _tokenPersistance.RemoveToken(WebApiCred.KeyChainTokenKey);
                OnFail(ret, new EventArgs());
                return false;
            }
            foodMarker.FoodMarkerId = ret.Obj.Id;

            return true;
        }

        public async Task<IEnumerable<FoodMarkerImageMeta>> SavePhotos(int id, List<byte[]> images)
        {
            var ret = await _restService.PostPhotos(id, images);
            if (!ret.IsSuccess)
            {
                _tokenPersistance.RemoveToken(WebApiCred.KeyChainTokenKey);
                OnFail(ret, new EventArgs());
                return null;
            }
            return ret.Obj;
        }

        public async Task<bool> DeleteFoodMarker(int foodMarkerId)
        {
            try
            {
                await _restService.DeletePhotos(foodMarkerId);
                await _restService.DeleteFoodMarker(foodMarkerId);
            } 
            catch
            {
                return false;
            }
            return true;
        }
    }

}
