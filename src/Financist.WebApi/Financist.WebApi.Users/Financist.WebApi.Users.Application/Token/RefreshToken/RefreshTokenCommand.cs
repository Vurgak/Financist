using Financist.WebApi.Users.Application.ViewModels;
using MediatR;

namespace Financist.WebApi.Users.Application.Token.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<TokensViewModel>;
