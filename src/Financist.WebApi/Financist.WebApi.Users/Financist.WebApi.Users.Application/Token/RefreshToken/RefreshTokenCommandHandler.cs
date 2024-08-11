using Financist.WebApi.Users.Application.Abstractions.Authentication;
using Financist.WebApi.Users.Application.Abstractions.Persistence.Stores;
using Financist.WebApi.Users.Application.ViewModels;
using MediatR;

namespace Financist.WebApi.Users.Application.Token.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokensViewModel>
{
    private readonly IRefreshTokenStore _refreshTokenStore;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public RefreshTokenCommandHandler(IRefreshTokenStore refreshTokenStore, IAccessTokenGenerator accessTokenGenerator)
    {
        _refreshTokenStore = refreshTokenStore;
        _accessTokenGenerator = accessTokenGenerator;
    }
    
    public async Task<TokensViewModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!await _refreshTokenStore.IsValidAsync(request.RefreshToken, cancellationToken))
            throw new Exception("Invalid refresh token");

        var oldRefreshToken = await _refreshTokenStore.GetAsync(request.RefreshToken, cancellationToken);
        var accessToken = _accessTokenGenerator.GenerateAccessToken(oldRefreshToken.SubjectId.ToString());
        var refreshToken = await _refreshTokenStore.GenerateTokenAsync(oldRefreshToken.SubjectId, cancellationToken);
        var tokens = new TokensViewModel
        {
            AccessToken = accessToken.Value,
            RefreshToken = refreshToken,
            TokenType = accessToken.Type,
            ExpiresIn = accessToken.ExpiresIn,
        };
        return tokens;
    }
}
