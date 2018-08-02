using System;
using System.Collections.Generic;
using FeedMapDTO;

namespace FeedMapDAL.Repository.Abstract
{
    public interface IFoodMarkerRepository
    {
        IEnumerable<FoodMarkerDTO> GetFoodMarkers();
        FoodMarkerDTO GetFoodMarker(int id);
        int Post(FoodMarkerDTO foodMarker);
        void Update(FoodMarkerDTO foodMarker, int id);
    }
}