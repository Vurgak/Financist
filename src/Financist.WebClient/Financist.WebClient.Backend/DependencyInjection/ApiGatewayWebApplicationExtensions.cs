using Financist.WebClient.Backend.ApiGateway;

namespace Financist.WebClient.Backend.DependencyInjection;

public static class ApiGatewayWebApplicationExtensions
{
    public static void MapApiGateway(this WebApplication app)
    {
        app.MapReverseProxy(ConfigureReverseProxy);
    }
    
    private static void ConfigureReverseProxy(IReverseProxyApplicationBuilder builder)
    {
        builder.Use(async (context, next) =>
        {
            var endpoint = context.GetEndpoint();

            var exclusionsService = context.RequestServices.GetRequiredService<EndpointRoutingExclusionService>();
            if (exclusionsService.IsExcluded(endpoint?.DisplayName, context.Request.Path))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Endpoint excluded");
                return;
            }
        
            await next();
        });
    }
}
