using DocumentManagement.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentManagement.API.Infrastructure.Impl.EfRepository
{
    public class DocumentEntityTypeConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(128);

            builder.Property(x => x.ContentType).HasMaxLength(128);

            builder.Property(x => x.BlobName).HasMaxLength(128);
        }
    }
}
