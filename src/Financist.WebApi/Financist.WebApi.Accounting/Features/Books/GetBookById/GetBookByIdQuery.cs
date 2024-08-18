using FastEndpoints;
using Financist.WebApi.Accounting.ViewModels;

namespace Financist.WebApi.Accounting.Features.Books.GetBookById;

public record GetBookByIdQuery(Guid BookId) : ICommand<BookViewModel?>;
