var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServiceDiscovery();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

builder.Services.AddRedis(builder.Configuration);

var app = builder.Build();
app.MapReverseProxy();
app.Run();
