using Financist.WebApi.Shared.System;
using Financist.WebApi.Users.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using NSubstitute;

namespace Financist.WebApi.Users.Infrastructure.UnitTests.Authentication;

public class AccessTokenGeneratorTests
{
    private const string TokenType = "Bearer";
    private const int Expiration = 1800;
    private const string Issuer = "https://financist.com/issuer";
    private const string Audience = "https://financist.com/audience";
    private const string Secret = "AjUodhRQ7MO9xTIivsR0yZx1qd7tG3Kc";
    private static readonly Dictionary<string, string?> ConfigurationItems = new()
    {
        { "AccessTokens:Type", TokenType },
        { "AccessTokens:Expiration", Expiration.ToString() },
        { "AccessTokens:Issuer", Issuer },
        { "AccessTokens:Audience", Audience },
        { "AccessTokens:Secret", Secret }
    };

    private static readonly string Subject = "817bd2bc-a404-4ea8-b217-2fbabd6e4df4";
    private static readonly DateTimeOffset IssuedAt = new DateTimeOffset(2024, 8, 10, 23, 4, 0, TimeSpan.Zero);

    private readonly AccessTokenGenerator _accessTokenGenerator;

    private readonly JsonWebTokenHandler _jwtHandler = new();
    
    public AccessTokenGeneratorTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(ConfigurationItems)
            .Build();
        
        var dateTimeOffsetProviderMock = Substitute.For<IDateTimeOffsetProvider>();
        dateTimeOffsetProviderMock.Now.Returns(IssuedAt);
        _accessTokenGenerator = new AccessTokenGenerator(configuration, dateTimeOffsetProviderMock);
    }
    
    [Fact]
    public void GenerateAccessToken_ReturnTokenWithTypeFromConfiguration()
    {
        var token = _accessTokenGenerator.GenerateAccessToken(Subject);

        Assert.Equal(TokenType, token.Type);
    }
    
    [Fact]
    public void GenerateAccessToken_ShouldGenerateTokenWithPassedSubject()
    {
        var token = _accessTokenGenerator.GenerateAccessToken(Subject);

        var decoded = (JsonWebToken) _jwtHandler.ReadToken(token.Value);
        Assert.Equal(Subject, decoded.Subject);
    }
    
    [Fact]
    public void GenerateAccessToken_ShouldGenerateTokenWithIssuerFromConfiguration()
    {
        var token = _accessTokenGenerator.GenerateAccessToken(Subject);

        var decoded = _jwtHandler.ReadToken(token.Value);
        Assert.Equal(Issuer, decoded.Issuer);
    }
    
    [Fact]
    public void GenerateAccessToken_ShouldGenerateTokenWithAudienceFromConfiguration()
    {
        var token = _accessTokenGenerator.GenerateAccessToken(Subject);

        var decoded = (JsonWebToken) _jwtHandler.ReadToken(token.Value);
        Assert.Single(decoded.Audiences);
        Assert.Equal(Audience, decoded.Audiences.First());
    }
}
