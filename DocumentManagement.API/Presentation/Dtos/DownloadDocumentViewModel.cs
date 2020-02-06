using System.IO;

namespace DocumentManagement.API.Presentation.Dtos
{
    public class DownloadDocumentViewModel
    {
        public Stream FileStream { get; set; }

        public string ContentType { get; set; }

        public string Name { get; set; }
    }
}