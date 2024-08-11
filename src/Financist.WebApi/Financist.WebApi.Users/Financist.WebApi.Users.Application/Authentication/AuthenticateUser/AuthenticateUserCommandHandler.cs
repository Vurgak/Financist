using Financist.WebApi.Users.Application.Abstractions.Authentication;
using Financist.WebApi.Users.Application.Abstractions.Cryptography;
using Financist.WebApi.Users.Application.Abstractions.Persistence;
using Financist.WebApi.Users.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financist.WebApi.Users.Application.Authentication.AuthenticateUser;

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, TokensViewModel>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IUsersDbContext _dbContext;

    public AuthenticateUserCommandHandler(
        IPasswordHasher passwordHasher,
        IAccessTokenGenerator accessTokenGenerator,
        IUsersDbContext dbContext)
    {
        _passwordHasher = passwordHasher;
        _accessTokenGenerator = accessTokenGenerator;
        _dbContext = dbContext;
    }
    
    public async Task<TokensViewModel> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == request.Email, cancellationToken);
        if (user is null)
            throw new Exception("User not found");

        var passwordIsValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!passwordIsValid)
            throw new Exception("Invalid password");

        var accessToken = _accessTokenGenerator.GenerateAccessToken(user.Id.ToString());
        var result = new TokensViewModel
        {
            AccessToken = accessToken.Value,
            TokenType = accessToken.Type,
            ExpiresIn = accessToken.ExpiresIn,
        };
        return result;
    }
}
