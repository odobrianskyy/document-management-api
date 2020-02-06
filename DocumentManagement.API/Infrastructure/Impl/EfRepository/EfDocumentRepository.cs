using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.API.Domain.Models;

namespace DocumentManagement.API.Infrastructure.Impl.EfRepository
{
    public class EfDocumentRepository : IDocumentRepository
    {
        private readonly DocumentDbContext _context;

        public EfDocumentRepository(DocumentDbContext context)
        {
            _context = context;
        }

        public IQueryable<Document> Query()
        {
            return _context.Documents;
        }

        public void Add(Document entity)
        {
            _context.Documents.Add(entity);
        }

        public void Remove(Document entity)
        {
            _context.Documents.Remove(entity);
        }

        public void Update(Document entity)
        {
            _context.Documents.Update(entity);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
