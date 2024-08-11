using Financist.WebApi.Users.Application.Abstractions.Authentication;
using Financist.WebApi.Users.Application.Abstractions.Cryptography;
using Financist.WebApi.Users.Application.Abstractions.Persistence;
using Financist.WebApi.Users.Application.Abstractions.Persistence.Stores;
using Financist.WebApi.Users.Application.ViewModels;
using Financist.WebApi.Users.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financist.WebApi.Users.Application.Authentication.AuthenticateUser;

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, TokensViewModel>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenStore _refreshTokenStore;
    private readonly IUsersDbContext _dbContext;

    public AuthenticateUserCommandHandler(
        IPasswordHasher passwordHasher,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenStore refreshTokenStore,
        IUsersDbContext dbContext)
    {
        _passwordHasher = passwordHasher;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenStore = refreshTokenStore;
        _dbContext = dbContext;
    }
    
    public async Task<TokensViewModel> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = await AuthenticateUserAsync(request, cancellationToken);
        var accessToken = _accessTokenGenerator.GenerateAccessToken(userId.ToString());
        var refreshToken = await _refreshTokenStore.GenerateTokenAsync(userId, cancellationToken);
        var result = new TokensViewModel
        {
            AccessToken = accessToken.Value,
            RefreshToken = refreshToken,
            TokenType = accessToken.Type,
            ExpiresIn = accessToken.ExpiresIn,
        };
        return result;
    }

    private async Task<Guid> AuthenticateUserAsync(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == request.Email, cancellationToken)
            ?? throw new Exception("User not found");

        var passwordIsValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!passwordIsValid)
            throw new Exception("Invalid password");
        
        return user.Id;
    }
}
