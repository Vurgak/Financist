using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Financist.WebApi.Accounting.Features.Books.GetBookById;

public class GetBookByIdEndpoint : Endpoint<GetBookByIdQuery>
{
    public override void Configure()
    {
        Get("/books/{BookId}");
        Options(x => x.WithTags("Books"));
    }

    public override async Task HandleAsync(GetBookByIdQuery request, CancellationToken ct)
    {
        var result = await request.ExecuteAsync(ct);

        if (result is not null)
            await SendOkAsync(result, ct);
        else
            await SendNotFoundAsync(ct);
    }
}
