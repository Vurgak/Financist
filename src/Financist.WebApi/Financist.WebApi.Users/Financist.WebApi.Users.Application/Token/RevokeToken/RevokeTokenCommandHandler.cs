using Financist.WebApi.Users.Application.Abstractions.Persistence.Stores;
using MediatR;

namespace Financist.WebApi.Users.Application.Token.RevokeToken;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
{
    private readonly IRefreshTokenStore _refreshTokenStore;

    public RevokeTokenCommandHandler(IRefreshTokenStore refreshTokenStore)
    {
        _refreshTokenStore = refreshTokenStore;
    }
    
    public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        if (!await _refreshTokenStore.IsValidAsync(request.RefreshToken, cancellationToken))
            throw new Exception("Invalid refresh token");

        await _refreshTokenStore.InvalidateAsync(request.RefreshToken, cancellationToken);
    }
}
