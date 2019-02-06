using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using FeedMapDAL.Helper;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;
using Microsoft.Extensions.Configuration;

namespace FeedMapDAL.Repository.Concrete
{
    public class AzureFileRepository : IMediaFileRepository
    {
        AzureStorageHandler _azureStorageHandler;

        public AzureFileRepository(IConfiguration configuration)
        {
            _azureStorageHandler = new AzureStorageHandler(configuration["AzureStorageConnectionString:FeedMapStorage"],
                                                           configuration["AzureStorageContainerReference:FeedMapReference"]);
        }

        public async Task<string> GetFile(FoodMarkerImageDataDTO foodMarkerImageDto, Stream stream)
        {
            string storageFileName = foodMarkerImageDto.Id.ToString() + "_" + foodMarkerImageDto.FileName;
            string contentType = await _azureStorageHandler.GetFile(storageFileName, stream);
            return contentType;
        }

        public string GetFileUrl(FoodMarkerImageDataDTO foodMarkerImageDto)
        {
            string storageFileName = foodMarkerImageDto.Id.ToString() + "_" + foodMarkerImageDto.FileName;
            return _azureStorageHandler.GetFileUrl(storageFileName);
        }

        public async Task PostFile(FoodMarkerImageDataDTO foodMarkerImageDto, string contentType, Stream stream)
        {

            await _azureStorageHandler.UploadFile(foodMarkerImageDto.Id.ToString()
                                                  + "_" + foodMarkerImageDto.FileName,
                                                  stream, contentType);
        }

        public async Task DeleteFile(FoodMarkerImageDataDTO foodMarkerImageDto)
        {
            string storageFileName = foodMarkerImageDto.Id.ToString() + "_" + foodMarkerImageDto.FileName;
            await _azureStorageHandler.DeleteFile(storageFileName);
        }
    }
}
