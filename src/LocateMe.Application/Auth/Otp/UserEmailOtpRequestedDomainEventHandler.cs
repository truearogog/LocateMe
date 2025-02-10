using LocateMe.Application.Abstractions.Cache;
using LocateMe.Application.Abstractions.Email;
using LocateMe.Application.Abstractions.Providers;
using LocateMe.Application.Abstractions.Services;
using LocateMe.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LocateMe.Application.Auth.Otp;

internal sealed class UserEmailOtpRequestedDomainEventHandler(
    IEmailService emailService,
    IEmailContentSevice emailContentSevice,
    UserManager<User> userManager,
    IDateTimeProvider dateTimeProvider,
    [FromKeyedServices("EmailTimeoutCache")] ICacheService cacheService)
    : INotificationHandler<UserEmailOtpRequestedDomainEvent>
{
    public async Task Handle(UserEmailOtpRequestedDomainEvent notification, CancellationToken cancellationToken)
    {
        string cacheName = nameof(UserEmailOtpRequestedDomainEventHandler) + notification.User.Email!;

        if (await cacheService.GetStringAsync(cacheName, cancellationToken) is null)
        {
            // send email
            string code = await userManager.GenerateTwoFactorTokenAsync(notification.User, "Email");

            var emailModel = new Email2FaModel(notification.User.UserName!, code, "LocateMe", dateTimeProvider.UtcNow.Year);
            string emailContent = await emailContentSevice.GetEmail2FaContentAsync(emailModel);
            var emailRequest = new EmailRequest([notification.User.Email!], emailContent, "LocateMe - 2FA Code");

            await emailService.SendEmailAsync(emailRequest, cancellationToken);

            await cacheService.SetStringAsync(cacheName, string.Empty, new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            }, cancellationToken);
        }
    }
}
