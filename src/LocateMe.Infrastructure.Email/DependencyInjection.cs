using LocateMe.Application.Abstractions.Email;
using Microsoft.Extensions.DependencyInjection;

namespace LocateMe.Infrastructure.Email;

public static class DependencyInjection
{
    public static IServiceCollection AddEmailInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IViewToStringRenderer, ViewToStringRenderer>();
        services.AddScoped<IEmailContentSevice, EmailContentSevice>();
        services.AddRazorTemplating();

        return services;
    }
}
