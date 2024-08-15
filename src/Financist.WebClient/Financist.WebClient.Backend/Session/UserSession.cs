using Financist.WebClient.Backend.Contracts;

namespace Financist.WebClient.Backend.Session;

public class UserSession
{
    public required string AccessToken { get; init; } = string.Empty;

    public required DateTimeOffset AccessTokenExpiration { get; init; }

    public required string RefreshToken { get; init; } = string.Empty;

    public static UserSession FromTokensResponse(TokensResponse tokens, DateTimeOffset now) => new()
    {
        AccessToken = tokens.AccessToken,
        AccessTokenExpiration = now.AddSeconds(tokens.ExpiresIn),
        RefreshToken = tokens.RefreshToken,
    };
}
