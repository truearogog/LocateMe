using LocateMe.Core;

namespace LocateMe.Domain.Users;

public sealed record UserRegisteredDomainEvent(Guid Id, User User) : DomainEvent(Id);
