using Financist.WebApi.Users.Application.Abstractions.Persistence;
using Financist.WebApi.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financist.WebApi.Users.Infrastructure.Persistence;

internal class UsersDbContext(DbContextOptions options) : DbContext(options), IUsersDbContext
{
    public const string Schema = "users";
    public const string MigrationsTableName = "_Migrations";
    
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        
        var assembly = typeof(UsersDbContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}
