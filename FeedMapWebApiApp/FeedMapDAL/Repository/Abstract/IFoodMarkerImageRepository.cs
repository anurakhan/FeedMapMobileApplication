using System;
using System.Collections.Generic;
using FeedMapDTO;
namespace FeedMapDAL.Repository.Abstract
{
    public interface IFoodMarkerImageRepository
    {
        IEnumerable<FoodMarkerImageDataDTO> GetFoodMarkerImages();
        FoodMarkerImageDataDTO GetFoodMarkerImage(int id);
        IEnumerable<FoodMarkerImageDataDTO> GetFoodMarkerImageByFoodMarkerId(int id);
        FoodMarkerImageDataDTO GetTopFoodMarkerImageByFoodMarkerId(int id);
        int Post(FoodMarkerImageDataDTO foodMarkerImg);
        void Update(FoodMarkerImageDataDTO foodMarkerImg, int id);
    }
}
