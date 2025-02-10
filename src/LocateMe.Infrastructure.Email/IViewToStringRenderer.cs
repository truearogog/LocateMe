namespace LocateMe.Infrastructure.Email;

public interface IViewToStringRenderer
{
    Task<string> RenderViewToStringAsync<TModel>(string viewPath, TModel model);
}
