namespace Financist.WebClient.Backend.Configuration;

public class AuthenticationConfiguration
{
    public string ServerUrl { get; set; } = string.Empty;
    public string RegisterEndpoint { get; set; } = string.Empty;
    public string AuthenticateEndpoint { get; set; } = string.Empty;
    public string RefreshTokenEndpoint { get; set; } = string.Empty;
    public string RevokeTokenEndpoint { get; set; } = string.Empty;
}
