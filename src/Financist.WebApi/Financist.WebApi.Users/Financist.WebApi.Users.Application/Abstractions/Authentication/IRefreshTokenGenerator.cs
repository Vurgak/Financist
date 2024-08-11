namespace Financist.WebApi.Users.Application.Abstractions.Authentication;

public interface IRefreshTokenGenerator
{
    int Expiration { get; }

    string GenerateRefreshToken();
}
