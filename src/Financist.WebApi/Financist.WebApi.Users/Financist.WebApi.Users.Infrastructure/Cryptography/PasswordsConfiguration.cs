namespace Financist.WebApi.Users.Infrastructure.Cryptography;

public class PasswordsConfiguration
{
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// The length of hash in bytes.
    /// </summary>
    public int HashLength { get; set; }

    /// <summary>
    /// The length of salt in bytes.
    /// </summary>
    public int SaltLength { get; set; }

    public int Parallelism { get; set; }

    public int MemorySize { get; set; }

    public int Iterations { get; set; }
}
