using LocateMe.Core;

namespace LocateMe.Domain.Users;

public sealed record UserPasswordResetRequestedDomainEvent(Guid Id, User User) : DomainEvent(Id);
