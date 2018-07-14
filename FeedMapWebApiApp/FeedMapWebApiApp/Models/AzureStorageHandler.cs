using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedMapWebApiApp.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;

namespace FeedMapWebApiApp.Models
{
    public class AzureStorageHandler
    {
        string m_StorageConnectionString;
        string m_ContainerRef;

        public async Task UploadFile(string fileName, Stream stream, string contentType)
        {
            var container = GetAzureContainer();
            var blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = contentType;
            await blockBlob.UploadFromStreamAsync(stream);
        }

        public async Task<string> GetFile(string fileName, Stream stream)
        {
            var container = GetAzureContainer();
            var blockBlob = container.GetBlobReference(fileName);
            await blockBlob.DownloadToStreamAsync(stream);
            return blockBlob.Properties.ContentType;
        }

        private CloudBlobContainer GetAzureContainer() 
        {
            var account = CloudStorageAccount.Parse(m_StorageConnectionString);
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(m_ContainerRef);
            return container;
        }

        public AzureStorageHandler(string connectionString, string containerRef)
        {
            m_StorageConnectionString = connectionString;
            m_ContainerRef = containerRef;
        }
    }
}
