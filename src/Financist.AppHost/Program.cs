var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var database = postgres.AddDatabase("database", "financist");

var webApi = builder.AddProject<Projects.Financist_WebApi_Bootstrapper>("webapi")
    .WithReference(database, "ApplicationDb")
    .WithExternalHttpEndpoints();

var webBff = builder.AddProject<Projects.Financist_WebClient_Backend>("webbff")
    .WithReference(webApi);

builder.AddNpmApp("webclient", "../Financist.WebClient/Financist.WebClient.Frontend")
    .WithReference(webBff)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();
