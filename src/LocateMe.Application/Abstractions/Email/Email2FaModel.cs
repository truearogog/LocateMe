namespace LocateMe.Application.Abstractions.Email;

public readonly record struct Email2FaModel(
    string RecipientName,
    string Code,
    string CompanyName,
    int CurrentYear);
