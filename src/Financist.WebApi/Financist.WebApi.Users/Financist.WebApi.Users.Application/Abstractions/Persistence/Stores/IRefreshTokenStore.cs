using Financist.WebApi.Users.Domain.Entities;

namespace Financist.WebApi.Users.Application.Abstractions.Persistence.Stores;

public interface IRefreshTokenStore
{
    Task<bool> IsValidAsync(string refreshToken, CancellationToken cancellationToken);
    
    Task<RefreshTokenEntity> GetAsync(string refreshToken, CancellationToken cancellationToken);

    Task<string> GenerateTokenAsync(Guid userId, CancellationToken cancellationToken);
}
