namespace LocateMe.Application.Abstractions.Email;

public readonly record struct EmailConfimationModel(
    string RecipientName,
    string ConfirmationLink,
    string CompanyName,
    int CurrentYear);
