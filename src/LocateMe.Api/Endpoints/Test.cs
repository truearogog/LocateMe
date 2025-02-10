using Microsoft.AspNetCore.Http.HttpResults;

namespace LocateMe.Api.Endpoints;

internal sealed class Test : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("test", Results<Ok<string>, UnauthorizedHttpResult> (CancellationToken cancellationToken) 
            => TypedResults.Ok("Hello, World!"))
        .RequireAuthorization();
    }
}
