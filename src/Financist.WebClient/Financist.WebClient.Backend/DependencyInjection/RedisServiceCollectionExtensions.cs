using StackExchange.Redis;

namespace Financist.WebClient.Backend.DependencyInjection;

public static class RedisServiceCollectionExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("SessionDb")!));

        services.AddScoped(serviceProvider =>
        {
            var multiplexer = serviceProvider.GetRequiredService<IConnectionMultiplexer>();
            return multiplexer.GetDatabase();
        });
        
        return services;
    }
}
