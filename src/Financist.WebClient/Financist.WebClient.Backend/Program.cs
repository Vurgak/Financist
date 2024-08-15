using Financist.WebClient.Backend.ApiGateway;
using Financist.WebClient.Backend.DependencyInjection;
using Financist.WebClient.Backend.Endpoints;
using Financist.WebClient.Backend.Session;
using Financist.WebClient.Backend.System;
using Financist.WebClient.Backend.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceDiscovery();
builder.Services.AddApiGateway(builder.Configuration);

builder.Services.AddRedis(builder.Configuration);

builder.Services.AddHttpClient();
builder.Services.ConfigureHttpClientDefaults(options => options.AddServiceDiscovery());

builder.Services.AddCookieAuthentication();

builder.Services.AddSingleton<EndpointRoutingExclusionService>();
builder.Services.AddScoped<IUserTokenService, UserTokenService>();
builder.Services.AddScoped<IUserSessionStore, UserSessionStore>();
builder.Services.AddScoped<IDateTimeOffsetProvider, DateTimeOffsetProvider>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthenticationEndpoints();
app.MapApiGateway();

app.Run();
