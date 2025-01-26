using LocateMe.Infrastructure.Authentication;
using LocateMe.Application.Abstractions.Providers;
using Microsoft.AspNetCore.Http;

namespace LocateMe.Infrastructure.Providers;

internal sealed class ActionContext(IHttpContextAccessor httpContextAccessor) : IActionContext
{
    public Lazy<string> SourceId { get; } = new(() =>
    {
        try
        {
            return httpContextAccessor.HttpContext?.User.GetUserId().ToString();
        }
        catch
        {
            return "system";
        }
    });
}
