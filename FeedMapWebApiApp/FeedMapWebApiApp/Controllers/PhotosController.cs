using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedMapWebApiApp.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class PhotosController : Controller
    {
        private DataAccess m_DataAccess;
        private string m_StorageConnectionString;

        public PhotosController(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration);
            m_StorageConnectionString = configuration["AzureStorageConnectionString:FeedMapStorage"];
        }

        [HttpGet("{id}")]
        public async Task<FileContentResult> Get(int id)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT TOP(1) CONCAT(CONVERT(varchar(12),FMP_ID),'_',FMP_FILE_NAME) ";
            sql += " FROM FoodMarkerPhotos ";
            sql += " WHERE FMP_FM_ID = @id ";
            sql += " ORDER BY FMP_ID ";

            string fileName = m_DataAccess.ExecuteScalar<string>(sql, sqlParams);

            AzureStorageHandler handler = new AzureStorageHandler(m_StorageConnectionString, "feedmapimages");

            using (Stream stream = new MemoryStream())
            {
                string contentType = await handler.GetFile(fileName, stream);
                byte[] buffer = ((MemoryStream)stream).ToArray();
                return File(buffer, contentType, fileName);
            }
        }
    }
}
