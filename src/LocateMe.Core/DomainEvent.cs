namespace LocateMe.Core;

internal abstract record DomainEvent(Guid Id) : IDomainEvent;