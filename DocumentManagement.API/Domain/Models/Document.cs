using System;

namespace DocumentManagement.API.Domain.Models
{
    public class Document
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }

        public string BlobName { get; set; }

        public int Order { get; set; }
    }
}
