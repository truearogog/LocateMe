namespace LocateMe.Application.Abstractions.Email;

public readonly record struct EmailPasswordResetModel(
    string RecipientName,
    string ResetLink,
    string CompanyName,
    int CurrentYear);
