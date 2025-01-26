namespace LocateMe.Application.Abstractions.Providers;

public interface IActionContext
{
    Lazy<string> SourceId { get; }
}
