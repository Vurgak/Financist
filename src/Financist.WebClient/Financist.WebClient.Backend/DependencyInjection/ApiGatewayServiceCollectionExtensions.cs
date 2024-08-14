using System.Security.Claims;
using Financist.WebClient.Backend.Session;
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
            var userSessionStore = services.ServiceProvider.GetRequiredService<IUserSessionStore>();

            var claims = context.HttpContext.User.Claims;
            var sessionId = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
            if (sessionId is null)
                return;
        
            var session = await userSessionStore.GetSessionDataAsync(sessionId);
            if (session is not null)
                context.ProxyRequest.Headers.Add("Authorization", $"Bearer {session.AccessToken}");
        });
    }
}
