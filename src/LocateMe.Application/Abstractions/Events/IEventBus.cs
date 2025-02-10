using LocateMe.Core;

namespace LocateMe.Application.Abstractions.Events;

public interface IEventBus
{
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
}
