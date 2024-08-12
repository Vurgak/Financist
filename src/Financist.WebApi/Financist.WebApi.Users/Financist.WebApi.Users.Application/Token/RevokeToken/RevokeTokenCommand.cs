using MediatR;

namespace Financist.WebApi.Users.Application.Token.RevokeToken;

public record RevokeTokenCommand(string RefreshToken) : IRequest;
