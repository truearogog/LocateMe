namespace LocateMe.Application.Abstractions.Email;

public interface IEmailContentSevice
{
    Task<string> GetEmailConfirmationContentAsync(EmailConfimationModel model);
    Task<string> GetEmail2FaContentAsync(Email2FaModel model);
    Task<string> GetEmailPasswordResetAsync(EmailPasswordResetModel model);
}
