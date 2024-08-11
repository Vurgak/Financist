namespace Financist.WebApi.Users.Infrastructure.Authentication;

public class AccessTokensConfiguration
{
    public string Type { get; set; } = string.Empty;
    public int Expiration { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
}
