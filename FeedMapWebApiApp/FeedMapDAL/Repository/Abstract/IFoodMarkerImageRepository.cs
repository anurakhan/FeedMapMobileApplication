using System;
using System.Collections.Generic;
using FeedMapDTO;
namespace FeedMapDAL.Repository.Abstract
{
    public interface IFoodMarkerImageRepository
    {
        IEnumerable<FoodMarkerImageDTO> GetFoodMarkerImages();
        FoodMarkerImageDTO GetFoodMarkerImage(int id);
        IEnumerable<FoodMarkerImageDTO> GetFoodMarkerImageByFoodMarkerId(int id);
        FoodMarkerImageDTO GetTopFoodMarkerImageByFoodMarkerId(int id);
        int Post(FoodMarkerImageDTO foodMarkerImg);
        void Update(FoodMarkerImageDTO foodMarkerImg, int id);
    }
}
