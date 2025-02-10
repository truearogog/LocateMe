using Razor.Templating.Core;

namespace LocateMe.Infrastructure.Email;

internal sealed class ViewToStringRenderer : IViewToStringRenderer
{
    public async Task<string> RenderViewToStringAsync<TModel>(string viewPath, TModel model)
    {
        return await RazorTemplateEngine.RenderAsync(viewPath, model);
    }
}
