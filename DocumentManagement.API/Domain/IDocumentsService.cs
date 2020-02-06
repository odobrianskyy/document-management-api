using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DocumentManagement.API.Domain.Models;

namespace DocumentManagement.API.Domain
{
    public interface IDocumentsService
    {
        Task<Document> GetByIdAsync(Guid id);

        Task<IList<Document>> GetOrderedListAsync();

        Task<int> GetMaxOrderAsync();

        Task<Stream> DownloadAsync(Document document);

        Task<Document> UploadAsync(Stream stream, string fileName, long fileSize, string contentType);

        Task ReOrderAsync(Document document, int newOrder);

        Task DeleteAsync(Document document);
    }
}
