namespace LocateMe.Core;

public abstract record DomainEvent(Guid Id) : IDomainEvent;
