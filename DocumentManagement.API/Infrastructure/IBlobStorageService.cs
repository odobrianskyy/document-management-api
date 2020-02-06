using System.IO;
using System.Threading.Tasks;

namespace DocumentManagement.API.Infrastructure
{
    public interface IBlobStorageService
    {
        Task UploadFileAsync(string blobName, Stream stream);

        Task<Stream> DownloadFileAsync(string blobName);

        Task DeleteFileAsync(string blobName);
    }
}
