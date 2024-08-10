using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Financist.WebApi.Shared.EntityFramework;

public static class MigrationDbContextExtensions
{
    public static IServiceCollection AddDbContextMigration<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        if (!EF.IsDesignTime)
            services.AddHostedService<DbContextMigrationService<TContext>>();
        return services;
    }
}

internal class DbContextMigrationService<TContext>(IServiceProvider serviceProvider) : IHostedService
    where TContext : DbContext
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<TContext>();
        
        await EnsureDatabaseExistsAsync(dbContext, cancellationToken);
        await MigrateDatabaseAsync(dbContext, cancellationToken);
    }

    private static async Task EnsureDatabaseExistsAsync(DbContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task MigrateDatabaseAsync(DbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
