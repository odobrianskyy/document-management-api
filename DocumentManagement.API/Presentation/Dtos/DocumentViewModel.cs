using System;

namespace DocumentManagement.API.Presentation.Dtos
{
    public class DocumentViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public long FileSize { get; set; }
    }
}
