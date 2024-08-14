namespace Financist.WebClient.Backend.Session;

public class UserSession
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
