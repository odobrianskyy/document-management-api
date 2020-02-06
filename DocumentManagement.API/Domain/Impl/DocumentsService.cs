using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.API.Domain.Models;
using DocumentManagement.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.API.Domain.Impl
{
    public class DocumentsService : IDocumentsService
    {
        private readonly IIdGenerator _idGenerator;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IDocumentRepository _documentRepository;

        public DocumentsService(
            IIdGenerator idGenerator, 
            IBlobStorageService blobStorageService, 
            IDocumentRepository documentRepository)
        {
            _idGenerator = idGenerator;
            _blobStorageService = blobStorageService;
            _documentRepository = documentRepository;
        }

        public Task<Document> GetByIdAsync(Guid id)
        {
            return _documentRepository.Query().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<Document>> GetOrderedListAsync()
        {
            return await _documentRepository.Query()
                .OrderBy(x => x.Order)
                .ToListAsync();
        }

        public async Task<int> GetMaxOrderAsync()
        {
            return await _documentRepository.Query().AnyAsync()
                ? await _documentRepository.Query().MaxAsync(x => x.Order)
                : 0;
        }

        public Task<Stream> DownloadAsync(Document document)
        {
            return _blobStorageService.DownloadFileAsync(document.BlobName);
        }

        public async Task<Document> UploadAsync(Stream stream, string fileName, long fileSize, string contentType)
        {
            var id = _idGenerator.NewId();
            var blobName = $"{id}{Path.GetExtension(fileName)}";

            await _blobStorageService.UploadFileAsync(blobName, stream);

            int maxOrder = await this.GetMaxOrderAsync();

            var newDocument = new Document
            {
                Id = id,
                BlobName = blobName,
                ContentType = contentType,
                FileSize = fileSize,
                Name = fileName,
                Order = maxOrder + 1
            };

            _documentRepository.Add(newDocument);
            await _documentRepository.SaveChangesAsync();

            return newDocument;
        }

        public async Task ReOrderAsync(Document document, int newOrder)
        {
            if (document.Order == newOrder)
            {
                // continue
            }
            else if (document.Order < newOrder)
            {
                await MoveDocumentDownTheList(document, newOrder);
            }
            else if (document.Order > newOrder)
            {
                await MoveDocumentUpTheList(document, newOrder);
            }
        }

        public async Task DeleteAsync(Document document)
        {
            var nextDocuments = _documentRepository.Query().Where(x => x.Order > document.Order);
            foreach (var nextDocument in nextDocuments)
            {
                // move each up the list by 1 position
                nextDocument.Order -= 1;
                _documentRepository.Update(nextDocument);
            }

            _documentRepository.Remove(document);
            await _documentRepository.SaveChangesAsync();

            // Fire and forget
            _blobStorageService.DeleteFileAsync(document.BlobName);
        }
        
        private async Task MoveDocumentDownTheList(Document document, int newPosition)
        {
            var currentPosition = document.Order;
            var nearbyDocuments = await _documentRepository.Query()
                .Where(x => x.Order > currentPosition && x.Order <= newPosition)
                .ToListAsync();

            foreach (var nearbyDocument in nearbyDocuments)
            {
                // move each up the list by 1 position
                nearbyDocument.Order -= 1;
                _documentRepository.Update(nearbyDocument);
            }

            // move down the list
            document.Order = newPosition;
            _documentRepository.Update(document);

            await _documentRepository.SaveChangesAsync();
        }

        private async Task MoveDocumentUpTheList(Document document, int newPosition)
        {
            var currentPosition = document.Order;
            var nearbyDocuments = await _documentRepository.Query()
                .Where(x => x.Order >= newPosition && x.Order < currentPosition)
                .ToListAsync();

            foreach (var nearbyDocument in nearbyDocuments)
            {
                // move each down the list by 1 position
                nearbyDocument.Order += 1;
                _documentRepository.Update(nearbyDocument);
            }

            // move up the list
            document.Order = newPosition;
            _documentRepository.Update(document);

            await _documentRepository.SaveChangesAsync();
        }
    }
}