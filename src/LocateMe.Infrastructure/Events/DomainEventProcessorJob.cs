using LocateMe.Core;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LocateMe.Infrastructure.Events;

internal sealed class DomainEventProcessorJob(
    InMemoryMessageQueue queue,
    IPublisher publisher,
    ILogger<DomainEventProcessorJob> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (IDomainEvent domainEvent in queue.Reader.ReadAllAsync(stoppingToken))
        {
            logger.LogDebug("Publishing {IntegrationEventId}", domainEvent.Id);

            await publisher.Publish(domainEvent, stoppingToken);

            logger.LogDebug("Processed {IntegrationEventId}", domainEvent.Id);
        }
    }
}
