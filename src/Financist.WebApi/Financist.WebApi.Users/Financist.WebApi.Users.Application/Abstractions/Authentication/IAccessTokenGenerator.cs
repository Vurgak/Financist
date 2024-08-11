namespace Financist.WebApi.Users.Application.Abstractions.Authentication;

public interface IAccessTokenGenerator
{
    AccessToken GenerateAccessToken(string subject);
}
