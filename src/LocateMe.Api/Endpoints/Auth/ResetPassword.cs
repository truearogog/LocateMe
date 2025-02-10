using System.Text;
using LocateMe.Api.Helpers;
using LocateMe.Application.Abstractions.Events;
using LocateMe.Domain.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace LocateMe.Api.Endpoints.Auth;

internal sealed class ResetPassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/reset-password", async Task<Results<Ok, ValidationProblem>> (
            Request request,
            IEventBus eventBus,
            UserManager<User> userManager,
            CancellationToken cancellationToken) =>
        {
            User user = await userManager.FindByEmailAsync(request.Email);
            if (user is null || !await userManager.IsEmailConfirmedAsync(user))
            {
                return ResultHelper.CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken()));
            }

            IdentityResult result;
            try
            {
                string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetCode));
                result = await userManager.ResetPasswordAsync(user, code, request.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());
            }

            if (!result.Succeeded)
            {
                return ResultHelper.CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        }).AllowAnonymous();
    }

    internal sealed record Request(string Email, string ResetCode, string NewPassword);
}
