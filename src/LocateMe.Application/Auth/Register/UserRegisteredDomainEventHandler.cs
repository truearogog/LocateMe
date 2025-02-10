using LocateMe.Application.Abstractions.Events;
using LocateMe.Domain.Users;
using MediatR;

namespace LocateMe.Application.Auth.Register;

internal sealed class UserRegisteredDomainEventHandler(IEventBus eventBus) : INotificationHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(new UserEmailConfirmationRequestedDomainEvent(Guid.NewGuid(), notification.User), cancellationToken);
    }
}
