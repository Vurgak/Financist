using MediatR;

namespace Financist.WebApi.Users.Integration;

public record UserRegisteredEvent(Guid UserId, string Email) : INotification;
