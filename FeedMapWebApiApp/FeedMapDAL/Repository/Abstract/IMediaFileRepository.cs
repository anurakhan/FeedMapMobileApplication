using System;
using System.IO;
using System.Threading.Tasks;
using FeedMapDTO;

namespace FeedMapDAL.Repository.Abstract
{
    public interface IMediaFileRepository
    {
        Task<string> GetFile(FoodMarkerImageDataDTO foodMarkerImageDto, Stream stream);

        Task PostFile(FoodMarkerImageDataDTO foodMarkerImageDto, string contentType, Stream stream);

        string GetFileUrl(FoodMarkerImageDataDTO foodMarkerImageDto);

        Task DeleteFile(FoodMarkerImageDataDTO foodMarkerImageDataDto);
    }
}
