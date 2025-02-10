using System.Collections.Specialized;
using System.Text;
using System.Web;
using LocateMe.Application.Abstractions.Email;
using LocateMe.Application.Abstractions.Providers;
using LocateMe.Application.Abstractions.Services;
using LocateMe.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace LocateMe.Application.Auth.ConfirmEmail;

internal sealed class UserEmailConfirmationRequestedDomainEventHandler(
    IEmailService emailService,
    IEmailContentSevice emailContentSevice,
    UserManager<User> userManager,
    IDateTimeProvider dateTimeProvider) 
    : INotificationHandler<UserEmailConfirmationRequestedDomainEvent>
{
    public async Task Handle(UserEmailConfirmationRequestedDomainEvent notification, CancellationToken cancellationToken)
    {
        // send email
        string code = await userManager.GenerateEmailConfirmationTokenAsync(notification.User);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["userId"] = notification.User.Id.ToString();
        queryString["code"] = code;
        string confirmationLink = "https://localhost:7014/auth/confirm-email?" + queryString.ToString();

        var emailModel = new EmailConfimationModel(notification.User.UserName!, confirmationLink, "LocateMe", dateTimeProvider.UtcNow.Year);
        string emailContent = await emailContentSevice.GetEmailConfirmationContentAsync(emailModel);
        var emailRequest = new EmailRequest([notification.User.Email!], emailContent, "LocateMe - Confirm your email");

        await emailService.SendEmailAsync(emailRequest, cancellationToken);
    }
}
