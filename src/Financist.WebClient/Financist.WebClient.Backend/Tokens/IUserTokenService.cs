using System.Security.Claims;

namespace Financist.WebClient.Backend.Tokens;

public interface IUserTokenService
{
    Task<string?> GetAccessTokenAsync(ClaimsPrincipal user, CancellationToken cancellationToken);

    Task RevokeRefreshToken(ClaimsPrincipal user, CancellationToken cancellationToken);
}
