using Financist.WebApi.Accounting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financist.WebApi.Accounting.Persistence.EntityTypeConfigurations;

public class BookEntityTypeConfiguration : IEntityTypeConfiguration<BookEntity>
{
    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(128);
    }
}
