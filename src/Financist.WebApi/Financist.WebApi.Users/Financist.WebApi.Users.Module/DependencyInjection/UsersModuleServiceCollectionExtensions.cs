using Financist.WebApi.Users.Application.DependencyInjection;
using Financist.WebApi.Users.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Financist.WebApi.Users.Module.DependencyInjection;

public static class UsersModuleServiceCollectionExtensions
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddUsersApplication()
            .AddUsersInfrastructure(configuration, environment.IsDevelopment());

        return services;
    }
}
