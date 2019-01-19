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
        private TokenPersistanceService _tokenPersistance;
        public event EventHandler OnFail;

        public FoodMarkerService()
        {
            _tokenPersistance = new TokenPersistanceService();
            _restService = new RestService();
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
    }


}
