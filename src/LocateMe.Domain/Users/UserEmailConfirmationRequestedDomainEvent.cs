using LocateMe.Core;

namespace LocateMe.Domain.Users;

public sealed record UserEmailConfirmationRequestedDomainEvent(Guid Id, User User) : DomainEvent(Id);
