using Financist.WebApi.Shared.System;
using Microsoft.Extensions.DependencyInjection;

namespace Financist.WebApi.Shared.DependencyInjection;

public static class SharedServiceCollectionExtensions
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeOffsetProvider, SystemDateTimeOffsetProvider>();
        return services;
    }
}
