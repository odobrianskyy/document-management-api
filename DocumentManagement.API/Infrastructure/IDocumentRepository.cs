using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.API.Domain.Models;

namespace DocumentManagement.API.Infrastructure
{
    public interface IDocumentRepository
    {
        IQueryable<Document> Query();

        void Add(Document entity);

        void Remove(Document entity);

        void Update(Document entity);

        Task SaveChangesAsync();
    }
}
