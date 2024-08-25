using Financist.WebApi.Users.Application.Abstractions.Cryptography;
using Financist.WebApi.Users.Application.Abstractions.Persistence;
using Financist.WebApi.Users.Domain.Entities;
using Financist.WebApi.Users.Integration;
using MediatR;

namespace Financist.WebApi.Users.Application.Authentication.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersDbContext _dbContext;
    private readonly IPublisher _publisher;

    public RegisterUserCommandHandler(IPasswordHasher passwordHasher, IUsersDbContext dbContext, IPublisher publisher)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
        _publisher = publisher;
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

        var userRegisteredEvent = new UserRegisteredEvent(user.Id, user.Email);
        await _publisher.Publish(userRegisteredEvent, cancellationToken);
    }
}
