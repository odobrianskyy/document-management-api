using DocumentManagement.API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.API.Infrastructure.Impl.EfRepository
{
    public class DocumentDbContext : DbContext
    {
        public DocumentDbContext(DbContextOptions<DocumentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DocumentEntityTypeConfiguration());
        }
    }
}
