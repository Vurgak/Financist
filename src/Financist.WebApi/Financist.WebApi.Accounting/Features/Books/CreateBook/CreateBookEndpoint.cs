using FastEndpoints;
using Financist.WebApi.Accounting.Features.Books.GetBookById;
using Financist.WebApi.Shared.Identity;
using Microsoft.AspNetCore.Http;

namespace Financist.WebApi.Accounting.Features.Books.CreateBook;

public class CreateBookEndpoint : Endpoint<CreateBookRequest>
{
    public override void Configure()
    {
        Post("books");
        Options(x => x.WithTags("Books"));
    }

    public override async Task HandleAsync(CreateBookRequest request, CancellationToken ct)
    {
        var command = MapToCommand(request);
        command.UserId = User.Identity!.GetSubjectId()!.Value;
        
        var book = await command.ExecuteAsync(ct);
        
        if (book is not null)
            await SendCreatedAtAsync<GetBookByIdEndpoint>(new { BookId = book.BookId }, book, cancellation: ct);
        else
            await SendResultAsync(Results.BadRequest());
    }

    private static CreateBookCommand MapToCommand(CreateBookRequest request) => new()
    {
        Name = request.Name,
    };
}
