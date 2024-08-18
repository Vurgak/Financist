using FastEndpoints;
using Financist.WebApi.Accounting.Entities;
using Financist.WebApi.Accounting.Persistence;
using Financist.WebApi.Accounting.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Financist.WebApi.Accounting.Features.Books.CreateBook;

public class CreateBookHandler : ICommandHandler<CreateBookCommand, BookViewModel?>
{
    private readonly AccountingDbContext _dbContext;

    public CreateBookHandler(AccountingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<BookViewModel?> ExecuteAsync(CreateBookCommand command, CancellationToken ct)
    {
        var exists = await _dbContext.Books.AnyAsync(book => book.Name == command.Name && book.UserId == command.UserId, ct);
        if (exists)
            return null;
        
        var bookEntity = new BookEntity
        {
            Name = command.Name,
            UserId = command.UserId,
        };
        
        await _dbContext.Books.AddAsync(bookEntity, ct);
        await _dbContext.SaveChangesAsync(ct);

        var result = MapToViewModel(bookEntity);
        return result;
    }

    private BookViewModel MapToViewModel(BookEntity entity) => new()
    {
        BookId = entity.Id,
        Name = entity.Name,
        UserId = entity.UserId, 
    };
}
