using Financist.WebApi.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financist.WebApi.Users.Infrastructure.Persistence.EntityTypeConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(e => e.Email)
            .HasMaxLength(320);

        builder.Property(e => e.PasswordHash)
            .HasMaxLength(48);
        
        builder.HasIndex(e => e.Email)
            .IsUnique();
    }
}
