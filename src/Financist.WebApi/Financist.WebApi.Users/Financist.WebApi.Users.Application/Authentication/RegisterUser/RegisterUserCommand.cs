using MediatR;

namespace Financist.WebApi.Users.Application.Authentication.RegisterUser;

public record RegisterUserCommand(string Email, string Password) : IRequest;
