using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentManagement.API.Application;
using DocumentManagement.API.Presentation.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Presentation.Controllers
{
    [Route("api/documents")]
    [Controller]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsApplicationService _documentsApplicationService;

        public DocumentsController(IDocumentsApplicationService documentsApplicationService)
        {
            _documentsApplicationService = documentsApplicationService;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Post(IFormFile file)
        {
            var id = await _documentsApplicationService.UploadAsync(file);
            return Ok(id);
        }

        [HttpGet]
        public async Task<ActionResult<IList<DocumentViewModel>>> Get()
        {
            var list = await _documentsApplicationService.GetListAsync();
            return Ok(list);
        }

        [HttpPost]
        [Route("reorder")]
        public async Task<IActionResult> ReOrder([FromBody] IList<OrderDocumentInputModel> list)
        {
            await _documentsApplicationService.ReOrderAsync(new OrderListInputModel { List = list });
            return Ok();
        }

        [HttpGet]
        [Route("{id}/download")]
        public async Task<IActionResult> Download(Guid id)
        {
            var downloadModel = await _documentsApplicationService.DownloadAsync(id);

            if (downloadModel == null)
            {
                return NotFound(id);
            }

            return File(downloadModel.FileStream, downloadModel.ContentType, downloadModel.Name);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeleted = await _documentsApplicationService.DeleteAsync(id);

            if (!isDeleted)
            {
                return Ok("Document was already deleted");
            }

            return Ok();
        }
    }
}