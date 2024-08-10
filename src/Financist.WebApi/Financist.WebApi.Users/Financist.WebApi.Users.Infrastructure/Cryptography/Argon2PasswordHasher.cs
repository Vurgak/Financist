using System.Security.Cryptography;
using System.Text;
using Financist.WebApi.Users.Application.Abstractions.Cryptography;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace Financist.WebApi.Users.Infrastructure.Cryptography;

internal class Argon2PasswordHasher : IPasswordHasher
{
    private readonly PasswordsConfiguration _configuration;

    public Argon2PasswordHasher(IConfiguration configuration)
    {
        var passwordsSection = configuration.GetSection("Passwords");
        var passwordsConfiguration = new PasswordsConfiguration();
        passwordsSection.Bind(passwordsConfiguration);
        _configuration = passwordsConfiguration;
    }
    
    public byte[] HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(_configuration.SaltLength);
        return HashPassword(password, salt);
    }

    public bool VerifyPassword(string password, byte[] expectedPasswordHash)
    {
        var passwordSalt = expectedPasswordHash[^_configuration.SaltLength..];
        var passwordHash = HashPassword(password, passwordSalt);
        return expectedPasswordHash.SequenceEqual(passwordHash);
    }

    private byte[] HashPassword(string password, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var secret = Encoding.UTF8.GetBytes(_configuration.Secret);
        var argon = new Argon2id(passwordBytes)
        {
            MemorySize = _configuration.MemorySize,
            Iterations = _configuration.Iterations,
            DegreeOfParallelism = _configuration.Parallelism,
            KnownSecret = secret,
            Salt = salt,
        };

        var passwordHash = argon.GetBytes(_configuration.HashLength);
        return [..passwordHash, ..salt];
    }
}
