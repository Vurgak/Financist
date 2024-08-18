using Financist.WebApi.Accounting.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financist.WebApi.Accounting.Persistence;

public class AccountingDbContext(DbContextOptions<AccountingDbContext> options) : DbContext(options)
{
    public const string Schema = "accounting";
    public const string MigrationsTableName = "_Migrations";

    public DbSet<BookEntity> Books => Set<BookEntity>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        
        var assembly = typeof(AccountingDbContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}
