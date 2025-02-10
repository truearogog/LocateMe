using LocateMe.Application.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace LocateMe.Infrastructure.Services;

internal sealed class TestEmailService(ILogger<TestEmailService> logger) : IEmailService
{
    public Task SendEmailAsync(EmailRequest mailRequest, CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Email recipients: {Recipients}; subject: {Subject}; body: {Body}", string.Join(",", mailRequest.Recipients), mailRequest.Subject, mailRequest.BodyHtml);

        return Task.CompletedTask;
    }
}
