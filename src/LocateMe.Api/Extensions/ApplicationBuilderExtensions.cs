namespace LocateMe.Api.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerWithUI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
