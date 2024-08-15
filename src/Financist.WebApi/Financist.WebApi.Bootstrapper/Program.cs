using System.Text;
using Financist.WebApi.Shared.DependencyInjection;
using Financist.WebApi.Users.Infrastructure.Authentication;
using Financist.WebApi.Users.Module.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var configuration = new AccessTokensConfiguration();
        builder.Configuration.GetSection("AccessTokens")
            .Bind(configuration);

        var secret = Encoding.UTF8.GetBytes(configuration.Secret);
        var signingKey = new SymmetricSecurityKey(secret);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration.Issuer,
            ValidAudience = configuration.Audience,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.FromSeconds(10),
        };
    });

builder.Services.AddSharedServices()
    .AddUsersModule(builder.Configuration, builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
