namespace Financist.WebApi.Users.Application.Abstractions.Authentication;

public class AccessToken
{
    public string Value { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}
