using Financist.WebApi.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financist.WebApi.Users.Infrastructure.Persistence.EntityTypeConfigurations;

public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.Property(e => e.Value)
            .HasMaxLength(64);

        builder.HasIndex(e => e.Value)
            .IsUnique();
    }
}
