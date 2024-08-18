namespace Financist.WebApi.Accounting.Entities;

public class BookEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid UserId { get; set; }
}
