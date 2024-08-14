namespace Financist.WebApi.Users.Domain.Entities;

public class RefreshTokenEntity
{
    public int Id { get; set; }

    public string Value { get; set; } = string.Empty;

    public Guid SubjectId { get; set; }

    public DateTimeOffset GeneratedOn { get; set; }

    public DateTimeOffset ValidUntil { get; set; }
}
