using Financist.WebApi.Accounting.Entities;
using Financist.WebApi.Accounting.Persistence;
using Financist.WebApi.Users.Integration;
using MediatR;

namespace Financist.WebApi.Accounting.EventHandlers;

public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
{
    private const string DefaultBookName = "Personal budget";
    
    private readonly AccountingDbContext _dbContext;

    public UserRegisteredEventHandler(AccountingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        var budget = new BookEntity
        {
            Id = Guid.NewGuid(),
            Name = DefaultBookName,
            UserId = notification.UserId,
        };
        
        await _dbContext.Books.AddAsync(budget, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
