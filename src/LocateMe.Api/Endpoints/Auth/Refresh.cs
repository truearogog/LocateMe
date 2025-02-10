using System.Security.Claims;
using LocateMe.Application.Abstractions.Authentication;
using LocateMe.Domain.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace LocateMe.Api.Endpoints.Auth;

internal sealed class Refresh : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/refresh", async Task<Results<Ok<Response>, UnauthorizedHttpResult>>(
            Request request,
            UserManager<User> userManager,
            IJwtTokenService jwtTokenService) =>
        {
            ClaimsPrincipal principal = jwtTokenService.ValidateRefreshToken(request.RefreshCode);

            if (principal == null)
            {
                return TypedResults.Unauthorized();
            }

            string userName = principal.Identity?.Name ?? string.Empty;
            User? user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return TypedResults.Unauthorized();
            }

            string accessToken = jwtTokenService.GenerateAccessToken(user);
            string refreshToken = jwtTokenService.GenerateRefreshToken(user);

            return TypedResults.Ok(new Response(accessToken, refreshToken));
        }).AllowAnonymous();
    }

    internal sealed record Request(string RefreshCode);
    internal sealed record Response(string AccessToken, string RefreshToken);
}
