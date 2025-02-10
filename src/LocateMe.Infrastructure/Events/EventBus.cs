using LocateMe.Application.Abstractions.Events;
using LocateMe.Core;

namespace LocateMe.Infrastructure.Events;

internal sealed class EventBus(InMemoryMessageQueue queue) : IEventBus
{
    public async Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default)
        where T : IDomainEvent
    {
        await queue.Writer.WriteAsync(domainEvent, cancellationToken);
    }
}
