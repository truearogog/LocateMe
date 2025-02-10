using LocateMe.Application.Abstractions.Cache;
using LocateMe.Application.Abstractions.Email;
using LocateMe.Application.Abstractions.Providers;
using LocateMe.Application.Abstractions.Services;
using LocateMe.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LocateMe.Application.Auth.PasswordReset;

internal sealed class UserPasswordResetRequestedDomainEventHandler(
    IEmailService emailService,
    IEmailContentSevice emailContentSevice,
    UserManager<User> userManager,
    IDateTimeProvider dateTimeProvider,
    [FromKeyedServices("EmailTimeoutCache")] ICacheService cacheService)
    : INotificationHandler<UserPasswordResetRequestedDomainEvent>
{
    public async Task Handle(UserPasswordResetRequestedDomainEvent notification, CancellationToken cancellationToken)
    {
        string cacheName = nameof(UserPasswordResetRequestedDomainEventHandler) + notification.User.Email!;

        if (await cacheService.GetStringAsync(cacheName, cancellationToken) is null)
        {
            // send email
            string code = await userManager.GeneratePasswordResetTokenAsync(notification.User);

            var emailModel = new EmailPasswordResetModel(notification.User.UserName!, code, "LocateMe", dateTimeProvider.UtcNow.Year);
            string emailContent = await emailContentSevice.GetEmailPasswordResetAsync(emailModel);
            var emailRequest = new EmailRequest([notification.User.Email!], emailContent, "LocateMe - Password Reset");

            await emailService.SendEmailAsync(emailRequest, cancellationToken);

            await cacheService.SetStringAsync(cacheName, string.Empty, new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            }, cancellationToken);
        }
    }
}
