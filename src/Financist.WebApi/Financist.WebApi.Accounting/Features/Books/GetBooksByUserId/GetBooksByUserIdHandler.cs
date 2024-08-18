using FastEndpoints;
using Financist.WebApi.Accounting.Entities;
using Financist.WebApi.Accounting.Persistence;
using Financist.WebApi.Accounting.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Financist.WebApi.Accounting.Features.Books.GetBooksByUserId;

public class GetBooksByUserIdHandler : ICommandHandler<GetBooksByUserIdQuery, IEnumerable<BookBaseViewModel>>
{
    private readonly AccountingDbContext _dbContext;

    public GetBooksByUserIdHandler(AccountingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<BookBaseViewModel>> ExecuteAsync(GetBooksByUserIdQuery query, CancellationToken ct)
    {
        var entities = await _dbContext.Books.AsNoTracking()
            .Where(book => book.UserId == query.UserId)
            .ToListAsync(ct);

        var results = MapToViewModels(entities);
        return results;
    }

    private IEnumerable<BookBaseViewModel> MapToViewModels(IEnumerable<BookEntity> entities) => entities.Select(
        entity => new BookBaseViewModel
        {
            BookId = entity.Id,
            Name = entity.Name,
        });
}
