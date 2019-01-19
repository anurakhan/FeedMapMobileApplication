using System;
using System.Collections.Generic;
using FeedMapBLL.Domain;

namespace FeedMapBLL.Services.Abstract
{
    public interface IPhotoService
    {
        FoodMarkerPhoto GetPhotoById(int id);

        IEnumerable<FoodMarkerPhoto> GetPhotosByFoodMarkerId(int id);
    }
}
