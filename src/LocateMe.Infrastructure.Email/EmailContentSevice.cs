using LocateMe.Application.Abstractions.Email;

namespace LocateMe.Infrastructure.Email;

internal sealed class EmailContentSevice(IViewToStringRenderer viewRenderer) : IEmailContentSevice
{
    public async Task<string> GetEmail2FaContentAsync(Email2FaModel model)
    {
        return await viewRenderer.RenderViewToStringAsync(@"Views\Email2Fa.cshtml", model);
    }

    public async Task<string> GetEmailConfirmationContentAsync(EmailConfimationModel model)
    {
        return await viewRenderer.RenderViewToStringAsync(@"Views\EmailConfirmation.cshtml", model);
    }

    public async Task<string> GetEmailPasswordResetAsync(EmailPasswordResetModel model)
    {
        return await viewRenderer.RenderViewToStringAsync(@"Views\EmailPasswordReset.cshtml", model);
    }
}
