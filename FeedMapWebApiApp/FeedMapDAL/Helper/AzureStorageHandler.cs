using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace FeedMapDAL.Helper
{
    public class AzureStorageHandler
    {
        string m_ContainerRef;
        string m_ConnectionString;

        public string ContainerRef
        {
            get
            {
                return m_ContainerRef;
            }
        }

        public string AzureStorageConnectionString
        {
            get
            {
                return m_ConnectionString;
            }
        }

        public AzureStorageHandler(string connectionString, string containerRef)
        {
            m_ConnectionString = connectionString;
            m_ContainerRef = containerRef;

        }

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

        public string GetFileUrl(string fileName)
        {
            var container = GetAzureContainer();
            var blockBlob = container.GetBlobReference(fileName);
            return blockBlob.Uri.AbsoluteUri;
        }

        private CloudBlobContainer GetAzureContainer()
        {
            var account = CloudStorageAccount.Parse(AzureStorageConnectionString);
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(m_ContainerRef);
            return container;
        }

    }
}
