using Financist.WebApi.Accounting.Persistence;
using Financist.WebApi.Shared.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Financist.WebApi.Accounting.DependencyInjection;

public static class AccountingModuleServiceCollectionExtensions
{
    public static IServiceCollection AddAccountingModule(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var migrateDatabase = environment.IsDevelopment();
        services.AddAccountingPersistence(configuration, migrateDatabase);
        
        return services;
    }

    private static void AddAccountingPersistence(
        this IServiceCollection services,
        IConfiguration configuration,
        bool migrateDatabase)
    {
        var connectionString = configuration.GetConnectionString("ApplicationDb");
        services.AddDbContext<AccountingDbContext>(options =>
            options.UseNpgsql(connectionString, ConfigureDbContext));
        
        if (migrateDatabase)
            services.AddDbContextMigration<AccountingDbContext>();
    }

    private static void ConfigureDbContext(NpgsqlDbContextOptionsBuilder options)
    {
        options.EnableRetryOnFailure();
        options.MigrationsHistoryTable(AccountingDbContext.MigrationsTableName, AccountingDbContext.Schema);
    }
}
