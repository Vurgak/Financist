﻿using Financist.WebApi.Users.Application.ViewModels;
using MediatR;

namespace Financist.WebApi.Users.Application.Authentication.AuthenticateUser;

public record AuthenticateUserCommand(string Email, string Password) : IRequest<TokensViewModel>;
