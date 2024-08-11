using System.Text;
using Financist.WebApi.Shared.System;
using Financist.WebApi.Users.Application.Abstractions.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Financist.WebApi.Users.Infrastructure.Authentication;

internal class AccessTokenGenerator : IAccessTokenGenerator
{
    private readonly AccessTokensConfiguration _configuration;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public AccessTokenGenerator(IConfiguration configuration, IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        var accessTokensConfiguration = new AccessTokensConfiguration();  
        configuration.GetSection("AccessTokens").Bind(accessTokensConfiguration);
        _configuration = accessTokensConfiguration;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }
    
    public AccessToken GenerateAccessToken(string subject)
    {
        var claims = new Dictionary<string, object>
        {
            { JwtRegisteredClaimNames.Sub, subject }
        };
        
        var secret = Encoding.UTF8.GetBytes(_configuration.Secret);
        var securityKey = new SymmetricSecurityKey(secret);
        var expiration = _dateTimeOffsetProvider.Now.DateTime.AddSeconds(_configuration.Expiration);
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration.Issuer,
            Audience = _configuration.Audience,
            Claims = claims,
            Expires = expiration,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(descriptor);
        return new AccessToken
        {
            Value = token,
            Type = "Bearer",
            ExpiresIn = _configuration.Expiration,
        };
    }
}
