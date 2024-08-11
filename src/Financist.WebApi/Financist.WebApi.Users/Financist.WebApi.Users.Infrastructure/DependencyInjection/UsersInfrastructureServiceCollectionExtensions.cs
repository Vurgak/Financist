using Financist.WebApi.Shared.EntityFramework;
using Financist.WebApi.Users.Application.Abstractions.Authentication;
using Financist.WebApi.Users.Application.Abstractions.Cryptography;
using Financist.WebApi.Users.Application.Abstractions.Persistence;
using Financist.WebApi.Users.Infrastructure.Authentication;
using Financist.WebApi.Users.Infrastructure.Cryptography;
using Financist.WebApi.Users.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Financist.WebApi.Users.Infrastructure.DependencyInjection;

public static class UsersInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services,
        IConfiguration configuration, bool migrateDatabase)
    {
        services.AddUsersPersistence(configuration, migrateDatabase);
        services.AddSingleton<IAccessTokenGenerator, AccessTokenGenerator>();
        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();
        return services;
    }

    private static void AddUsersPersistence(this IServiceCollection services, IConfiguration configuration, bool shouldMigrate)
    {
        var connectionString = configuration.GetConnectionString("ApplicationDb");
        services.AddDbContext<IUsersDbContext, UsersDbContext>(options =>
            options.UseNpgsql(connectionString, ConfigureDbContext));
        
        if (shouldMigrate)
            services.AddDbContextMigration<UsersDbContext>();
    }

    private static void ConfigureDbContext(NpgsqlDbContextOptionsBuilder options)
    {
        options.EnableRetryOnFailure();
        options.MigrationsHistoryTable(UsersDbContext.MigrationsTableName, UsersDbContext.Schema);
    }
}
