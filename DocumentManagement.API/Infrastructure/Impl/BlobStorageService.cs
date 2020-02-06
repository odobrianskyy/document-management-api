using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace DocumentManagement.API.Infrastructure.Impl
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;

        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public async Task UploadFileAsync(string blobName, Stream stream)
        {
            var blobClient = await GetBlobClientAsync(blobName);

            using (stream)
            {
                await blobClient.UploadAsync(stream);
            }
        }

        public async Task<Stream> DownloadFileAsync(string blobName)
        {
            var blobClient = await GetBlobClientAsync(blobName);
            var downloadResult = await blobClient.DownloadAsync();
            return downloadResult.Value.Content;
        }

        public async Task DeleteFileAsync(string blobName)
        {
            var blobClient = await GetBlobClientAsync(blobName);
            await blobClient.DeleteAsync();
        }

        private async Task<BlobClient> GetBlobClientAsync(string blobName)
        {
            var connectionString = _configuration.GetConnectionString("AZURE_STORAGE_CONNECTION_STRING");
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient("document-management-files");
            await containerClient.CreateIfNotExistsAsync();
            return containerClient.GetBlobClient(blobName);
        }
    }
}
