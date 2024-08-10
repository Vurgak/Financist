using Financist.WebApi.Users.Application.Abstractions.Cryptography;
using Financist.WebApi.Users.Application.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financist.WebApi.Users.Application.Authentication.AuthenticateUser;

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersDbContext _dbContext;

    public AuthenticateUserCommandHandler(IPasswordHasher passwordHasher, IUsersDbContext dbContext)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
    }
    
    public async Task Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == request.Email, cancellationToken);
        if (user is null)
            throw new Exception("User not found");

        var passwordIsValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!passwordIsValid)
            throw new Exception("Invalid password");
    }
}
