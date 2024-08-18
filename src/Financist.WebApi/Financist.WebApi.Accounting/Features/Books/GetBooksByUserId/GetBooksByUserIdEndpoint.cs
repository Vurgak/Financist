using FastEndpoints;
using Financist.WebApi.Accounting.ViewModels;
using Financist.WebApi.Shared.Identity;
using Microsoft.AspNetCore.Http;

namespace Financist.WebApi.Accounting.Features.Books.GetBooksByUserId;

public class GetBooksByUserIdEndpoint : Endpoint<GetBooksByUserIdQuery, IEnumerable<BookBaseViewModel>>
{
    public override void Configure()
    {
        Get("/users/{UserId}/books");
        Options(x => x.WithTags("Books"));
    }

    public override async Task HandleAsync(GetBooksByUserIdQuery request, CancellationToken ct)
    {
        if (User.Identity?.GetSubjectId() != request.UserId)
        {
            await SendForbiddenAsync(ct);
            return;
        }

        var books = await request.ExecuteAsync(ct);
        await SendOkAsync(books, ct);
    }
}
