﻿using System;
using System.Collections.Generic;
using FeedMapBLL.Domain;

namespace FeedMapBLL.Services.Abstract
{
    public interface IFoodMarkerService
    {
        IEnumerable<FoodMarker> GetFoodMarkers();

        FoodMarker GetFoodMarker(int id);

        int PostFoodMarker(FoodMarker foodMarker);

        IEnumerable<FullFoodAndGeoData> GetCompleteFoodData();

        FullFoodAndGeoData GetCompleteFoodDataById(int id);

        int PostFullFoodAndGeoData(FullFoodAndGeoData fullFoodAndGeoData, User user);

        IEnumerable<FullFoodAndGeoData> GetCompleteFoodDataByUserId(int id);

        void DeleteFoodMarker(int id);
    }
}
