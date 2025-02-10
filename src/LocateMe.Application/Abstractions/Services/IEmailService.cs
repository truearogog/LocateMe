namespace LocateMe.Application.Abstractions.Services;

public readonly record struct EmailRequest(List<string> Recipients, string BodyHtml, string Subject);

public interface IEmailService
{
    Task SendEmailAsync(EmailRequest mailRequest, CancellationToken cancellationToken = default);
}
