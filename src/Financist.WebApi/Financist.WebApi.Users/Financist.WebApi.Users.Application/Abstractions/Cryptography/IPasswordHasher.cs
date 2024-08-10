namespace Financist.WebApi.Users.Application.Abstractions.Cryptography;

public interface IPasswordHasher
{
    byte[] HashPassword(string password);

    bool VerifyPassword(string password, byte[] expectedPasswordHash);
}
