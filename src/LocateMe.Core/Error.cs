namespace LocateMe.Core;

public record Error(string Code, string Description, ErrorType Type, string Instance)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure, string.Empty);
    public static readonly Error NullValue = new("General.Null", "Null value was provided", ErrorType.Failure, string.Empty);
    public static readonly Error FeatureNotFound = new("General.FeatureNotFound", "Feature not found", ErrorType.NotFound, string.Empty);

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure, string.Empty);

    public static Error NotFound(string code, string description, string instance) =>
        new(code, description, ErrorType.NotFound, instance);

    public static Error Problem(string code, string description) =>
        new(code, description, ErrorType.Problem, string.Empty);

    public static Error Conflict(string code, string description, string instance = "") =>
        new(code, description, ErrorType.Conflict, instance);
}
