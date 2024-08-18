using FastEndpoints;
using Financist.WebApi.Accounting.ViewModels;

namespace Financist.WebApi.Accounting.Features.Books.CreateBook;

public class CreateBookCommand : ICommand<BookViewModel?>
{
    public required string Name { get; init; }

    public Guid UserId { get; set; }
};
