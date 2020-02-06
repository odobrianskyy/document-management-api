using System.Collections.Generic;

namespace DocumentManagement.API.Presentation.Dtos
{
    public class OrderListInputModel
    {
        public IList<OrderDocumentInputModel> List { get; set; }
    }
}
