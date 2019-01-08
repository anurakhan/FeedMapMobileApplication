using System;
using System.IO;
using System.Threading.Tasks;
using FeedMapDTO;

namespace FeedMapDAL.Repository.Abstract
{
    public interface IMediaFileRepository
    {
        Task<string> GetFile(FoodMarkerImageDTO foodMarkerImageDto, Stream stream);

        Task PostFile(FoodMarkerImageDTO foodMarkerImageDto, string contentType, Stream stream);

        string GetFileUrl(FoodMarkerImageDTO foodMarkerImageDto);
    }
}
