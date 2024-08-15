using Financist.WebClient.Backend.Tokens;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Financist.WebClient.Backend.DependencyInjection;

public static class ApiGatewayServiceCollectionExtensions
{
    public static IServiceCollection AddApiGateway(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddReverseProxy()
            .AddServiceDiscoveryDestinationResolver()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"))
            .AddTransforms(ConfigureTransforms);
        
        return services;
    }
    
    private static void ConfigureTransforms(TransformBuilderContext builder)
    {
        builder.AddRequestTransform(async context =>
        {
            using var services = builder.Services.CreateScope();
            var userTokenService = services.ServiceProvider.GetRequiredService<IUserTokenService>();

            var user = context.HttpContext.User;
            var accessToken = await userTokenService.GetAccessTokenAsync(user, context.CancellationToken);
            if (accessToken is not null)
                context.ProxyRequest.Headers.Add("Authorization", $"Bearer {accessToken}");
        });
    }
}
