using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Financist.WebApi.Users.Application.DependencyInjection;

public static class UsersApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddUsersApplication(this IServiceCollection services)
    {
        var assembly = typeof(UsersApplicationServiceCollectionExtensions).Assembly;
        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}
