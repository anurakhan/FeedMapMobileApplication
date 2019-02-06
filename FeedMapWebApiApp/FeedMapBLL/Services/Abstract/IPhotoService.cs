using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FeedMapBLL.Domain;

namespace FeedMapBLL.Services.Abstract
{
    public interface IPhotoService
    {
        FoodMarkerPhoto GetPhotoById(int id);

        IEnumerable<FoodMarkerPhoto> GetPhotosByFoodMarkerId(int id);

        Task<FoodMarkerPhoto> PostPhotoById(FoodMarkerImageData foodMarkerImageData,
                           string contentType, Stream stream);

        Task DeletePhotosByFoodMarkerId(int id);
    }
}
