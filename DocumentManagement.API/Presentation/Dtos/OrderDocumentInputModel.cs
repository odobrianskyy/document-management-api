using System;

namespace DocumentManagement.API.Presentation.Dtos
{
    public class OrderDocumentInputModel
    {
        public Guid Id { get; set; }

        public int Order { get; set; }
    }
}
