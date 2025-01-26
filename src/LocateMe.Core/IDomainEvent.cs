using MediatR;

namespace LocateMe.Core;

public interface IDomainEvent : INotification
{
    Guid Id { get; init; }
}
