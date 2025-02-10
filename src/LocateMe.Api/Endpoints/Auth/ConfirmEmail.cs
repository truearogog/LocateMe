using System.Text;
using LocateMe.Domain.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace LocateMe.Api.Endpoints.Auth;

internal sealed class ConfirmEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("auth/confirm-email", async Task<Results<Ok, UnauthorizedHttpResult>> (
            string userId,
            string code,
            UserManager<User> userManager) =>
        {
            User user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return TypedResults.Unauthorized();
            }

            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return TypedResults.Unauthorized();
            }

            IdentityResult result = await userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                return TypedResults.Unauthorized();
            }

            await userManager.SetTwoFactorEnabledAsync(user, true);

            return TypedResults.Ok();
        }).AllowAnonymous();
    }
}
