using Financist.WebApi.Users.Application.Abstractions.Cryptography;
using Financist.WebApi.Users.Application.Abstractions.Persistence;
using Financist.WebApi.Users.Domain.Entities;
using MediatR;

namespace Financist.WebApi.Users.Application.Authentication.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersDbContext _dbContext;

    public RegisterUserCommandHandler(IPasswordHasher passwordHasher, IUsersDbContext dbContext)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
    }
    
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = passwordHash,
        };
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
