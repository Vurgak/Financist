using System.Security.Cryptography;
using Financist.WebApi.Users.Application.Abstractions.Authentication;
using Microsoft.Extensions.Configuration;

namespace Financist.WebApi.Users.Infrastructure.Authentication;

public class RefreshTokenGenerator(IConfiguration configuration) : IRefreshTokenGenerator
{
    public int Expiration { get; } = configuration.GetValue<int>("RefreshTokens:Expiration");

    private readonly int _tokenLength = configuration.GetValue<int>("RefreshTokens:Length");
    private readonly string _alphabet = configuration.GetValue<string>("RefreshTokens:Alphabet") ?? string.Empty;

    public string GenerateRefreshToken() => RandomNumberGenerator.GetString(_alphabet, _tokenLength);
}
