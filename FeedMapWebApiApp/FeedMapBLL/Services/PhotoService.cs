using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FeedMapBLL.Domain;
using FeedMapBLL.Services.Abstract;
using FeedMapDAL;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;

namespace FeedMapBLL.Services
{
    public class PhotoService : IPhotoService
    {
        private IFoodMarkerImageRepository _repoImageMeta;
        private IMediaFileRepository _repoImageFile;
        private IMapper _mapper;

        public PhotoService(RepositoryPayload repoPayload,
                                   IMapper mapper)
        {
            _repoImageMeta = repoPayload.GetFoodMarkerImageRepository();
            _repoImageFile = repoPayload.GetFileRepository();
            _mapper = mapper;
        }

        public IEnumerable<FoodMarkerPhoto> GetPhotosByFoodMarkerId(int id)
        {
            var imageMetas = _repoImageMeta.GetFoodMarkerImageByFoodMarkerId(id);

            if (imageMetas == null || !imageMetas.Any()) return null;

            List<FoodMarkerPhoto> lstFoodMarkerImageData =
                new List<FoodMarkerPhoto>();

            foreach (var imageMeta in imageMetas)
            {
                lstFoodMarkerImageData.Add(
                    new FoodMarkerPhoto
                    {
                        ImageUrl = _repoImageFile.GetFileUrl(imageMeta),
                        ImageRank = (imageMeta.ImageRank.HasValue ?
                                 imageMeta.ImageRank.Value : 2)
                    });
            }

            return lstFoodMarkerImageData;
        }

        public FoodMarkerPhoto GetPhotoById(int id)
        {
            FoodMarkerImageDataDTO imageMeta = _repoImageMeta.GetFoodMarkerImage(id);

            if (imageMeta == null) return null;

            return new FoodMarkerPhoto {
                ImageUrl = _repoImageFile.GetFileUrl(imageMeta),
                ImageRank = (imageMeta.ImageRank.HasValue ?
                                 imageMeta.ImageRank.Value : 2)
            };
        }
    }
}
