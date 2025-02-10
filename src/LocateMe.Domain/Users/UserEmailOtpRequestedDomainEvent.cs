using LocateMe.Core;

namespace LocateMe.Domain.Users;

public sealed record UserEmailOtpRequestedDomainEvent(Guid Id, User User) : DomainEvent(Id);
