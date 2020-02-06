using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.API.Application.Validation;
using DocumentManagement.API.Domain;
using DocumentManagement.API.Domain.Exceptions;
using DocumentManagement.API.Presentation.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace DocumentManagement.API.Application.Impl
{
    public class DocumentsApplicationService : IDocumentsApplicationService
    {
        private readonly IDocumentsService _documentsService;

        public DocumentsApplicationService(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        public async Task<Guid> UploadAsync(IFormFile file)
        {
            var validator = new FormFileValidator();
            validator.ValidateAndThrow(file);

            var document = await _documentsService.UploadAsync(
                file.OpenReadStream(),
                file.FileName,
                file.Length,
                file.ContentType);

            return document.Id;
        }

        public async Task<IList<DocumentViewModel>> GetListAsync()
        {
            var documents = await _documentsService.GetOrderedListAsync();

            var viewModels = documents.Select(x => new DocumentViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    FileSize = x.FileSize,
                    Location = $"/api/documents/{x.Id}/download"
                })
                .ToList();

            return viewModels;
        }

        public async Task ReOrderAsync(OrderListInputModel listInputModel)
        {
            int maxOrder = await _documentsService.GetMaxOrderAsync();

            var validator = new OrderListInputModelValidator(maxOrder);
            validator.ValidateAndThrow(listInputModel);
            
            var orderedInputModels = listInputModel.List.OrderBy(x => x.Order).ToList();

            foreach (var inputModel in orderedInputModels)
            {
                var document = await _documentsService.GetByIdAsync(inputModel.Id);

                if (document == null)
                {
                    throw new NotFoundException($"Document with given id {inputModel.Id} not found");
                }

                await _documentsService.ReOrderAsync(document, inputModel.Order);
            }
        }

        public async Task<DownloadDocumentViewModel> DownloadAsync(Guid id)
        {
            var document = await _documentsService.GetByIdAsync(id);

            if (document == null)
            {
                throw new NotFoundException($"Document with given id {id} not found");
            }

            var downloadStream = await _documentsService.DownloadAsync(document);

            return new DownloadDocumentViewModel
            {
                Name = document.Name,
                ContentType = document.ContentType,
                FileStream = downloadStream
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var document = await _documentsService.GetByIdAsync(id);

            if (document == null)
            {
                return false;
            }

            await _documentsService.DeleteAsync(document);

            return true;
        }
    }
}