using FastEndpoints;
using FluentValidation;

namespace Financist.WebApi.Accounting.Features.Books.CreateBook;

public class CreateBookCommandValidator : Validator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 128);
    }
}
