using Financist.WebClient.Backend.DependencyInjection;
using Financist.WebClient.Backend.Endpoints;
using Financist.WebClient.Backend.Services;
using Financist.WebClient.Backend.Session;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceDiscovery();
builder.Services.AddApiGateway(builder.Configuration);

builder.Services.AddHttpClient();
builder.Services.ConfigureHttpClientDefaults(options => options.AddServiceDiscovery());

builder.Services.AddCookieAuthentication();

builder.Services.AddRedis(builder.Configuration);

builder.Services.AddSingleton<EndpointRoutingExclusionService>();

builder.Services.AddScoped<IUserSessionStore, UserSessionStore>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthenticationEndpoints();
app.MapApiGateway();

app.Run();
