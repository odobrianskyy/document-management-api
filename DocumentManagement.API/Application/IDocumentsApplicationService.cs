using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentManagement.API.Presentation.Dtos;
using Microsoft.AspNetCore.Http;

namespace DocumentManagement.API.Application
{
    public interface IDocumentsApplicationService
    {
        Task<Guid> UploadAsync(IFormFile file);

        Task<IList<DocumentViewModel>> GetListAsync();

        Task ReOrderAsync(OrderListInputModel listInputModel);

        Task<DownloadDocumentViewModel> DownloadAsync(Guid id);

        Task<bool> DeleteAsync(Guid id);
    }
}
