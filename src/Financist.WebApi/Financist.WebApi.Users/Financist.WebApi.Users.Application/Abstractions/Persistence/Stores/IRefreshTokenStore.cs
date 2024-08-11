namespace Financist.WebApi.Users.Application.Abstractions.Persistence.Stores;

public interface IRefreshTokenStore
{
    Task<bool> IsValidAsync(string refreshToken, CancellationToken cancellationToken);

    Task<string> GenerateTokenAsync(Guid userId, CancellationToken cancellationToken);
}
