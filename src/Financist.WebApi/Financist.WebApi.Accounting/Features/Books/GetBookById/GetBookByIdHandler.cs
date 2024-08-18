using FastEndpoints;
using Financist.WebApi.Accounting.Entities;
using Financist.WebApi.Accounting.Persistence;
using Financist.WebApi.Accounting.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Financist.WebApi.Accounting.Features.Books.GetBookById;

public class GetBookByIdHandler : ICommandHandler<GetBookByIdQuery, BookViewModel?>
{
    private readonly AccountingDbContext _dbContext;

    public GetBookByIdHandler(AccountingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<BookViewModel?> ExecuteAsync(GetBookByIdQuery query, CancellationToken ct)
    {
        var entity = await _dbContext.Books.AsNoTracking()
            .FirstOrDefaultAsync(book => book.Id == query.BookId, ct);

        if (entity is null)
            return null;

        var result = MapToViewModel(entity);
        return result;
    }

    private static BookViewModel MapToViewModel(BookEntity entity) => new()
    {
        BookId = entity.Id,
        Name = entity.Name,
        UserId = entity.UserId,
    };
}