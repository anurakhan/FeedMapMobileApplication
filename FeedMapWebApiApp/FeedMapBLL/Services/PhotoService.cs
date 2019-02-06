using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                        ImageId = imageMeta.Id,
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

        public async Task<FoodMarkerPhoto> PostPhotoById(FoodMarkerImageData foodMarkerImageData, 
                                  string contentType, Stream stream)
        {
            var postImageMetaDto = _mapper.Map<FoodMarkerImageDataDTO>(foodMarkerImageData);
            foodMarkerImageData.Id = postImageMetaDto.Id = _repoImageMeta.Post(postImageMetaDto);

            await _repoImageFile.PostFile(postImageMetaDto, contentType, stream);
            return new FoodMarkerPhoto
            {
                ImageUrl = _repoImageFile.GetFileUrl(postImageMetaDto),
                ImageRank = (foodMarkerImageData.ImageRank.HasValue ?
                             foodMarkerImageData.ImageRank.Value : 2)
            };
        }

        public async Task DeletePhotosByFoodMarkerId(int id)
        {
            var imageMetas = _repoImageMeta.GetFoodMarkerImageByFoodMarkerId(id);
            _repoImageMeta.DeleteByFoodMarker(id);

            foreach (var imageMeta in imageMetas)
            {
                await _repoImageFile.DeleteFile(imageMeta);   
            }
        }

    }
}