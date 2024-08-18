using FastEndpoints;
using Financist.WebApi.Accounting.ViewModels;

namespace Financist.WebApi.Accounting.Features.Books.GetBooksByUserId;

public record GetBooksByUserIdQuery(Guid UserId) : ICommand<IEnumerable<BookBaseViewModel>>;
