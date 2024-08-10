namespace Financist.WebApi.Users.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public byte[] PasswordHash { get; set; }
}
