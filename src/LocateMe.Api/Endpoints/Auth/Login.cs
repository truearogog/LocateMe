using LocateMe.Api.Helpers;
using LocateMe.Application.Abstractions.Authentication;
using LocateMe.Application.Abstractions.Email;
using LocateMe.Application.Abstractions.Events;
using LocateMe.Application.Abstractions.Providers;
using LocateMe.Application.Abstractions.Services;
using LocateMe.Domain.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace LocateMe.Api.Endpoints.Auth;

internal sealed class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/login", async Task<Results<Ok<Response>, ValidationProblem, ForbidHttpResult>> (
            Request request,
            UserManager<User> userManager,
            IJwtTokenService jwtTokenService,
            CancellationToken cancellationToken,
            IEmailService emailService,
            IEmailContentSevice emailContentService,
            IDateTimeProvider dateTimeProvider,
            IEventBus eventBus) =>
        {
            User user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return ResultHelper.CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(request.Email)));
            }

            bool passwordIsValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordIsValid)
            {
                return ResultHelper.CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.PasswordMismatch()));
            }

            bool isTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user);

            if (isTwoFactorEnabled)
            {
                if (!string.IsNullOrWhiteSpace(request.TwoFactorRecoveryCode))
                {
                    string recoveryCode = request.TwoFactorRecoveryCode.Replace(" ", string.Empty);
                    IdentityResult reedemResult = await userManager.RedeemTwoFactorRecoveryCodeAsync(user, recoveryCode);
                    if (!reedemResult.Succeeded)
                    {
                        return ResultHelper.CreateValidationProblem(reedemResult);
                    }
                }
                else
                {
                    bool no2faCodeSupplied = string.IsNullOrWhiteSpace(request.TwoFactorCode);

                    IList<string> validProviders = await userManager.GetValidTwoFactorProvidersAsync(user);

                    if (no2faCodeSupplied && validProviders.Contains("Email"))
                    {
                        await eventBus.PublishAsync(new UserEmailOtpRequestedDomainEvent(Guid.NewGuid(), user), cancellationToken);

                        return TypedResults.Forbid();
                    }

                    // If we get here, either:
                    // - The user did supply a 2FA code, OR
                    // - The user doesn’t have Email as a valid provider (but might have Authenticator)
                    if (string.IsNullOrEmpty(request.TwoFactorCode))
                    {
                        // If there's still no code, it's an error
                        return ResultHelper.CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken()));
                    }

                    // Now attempt verification with Email or Authenticator
                    bool twoFactorSuccess = false;

                    if (validProviders.Contains("Email"))
                    {
                        twoFactorSuccess = await userManager.VerifyTwoFactorTokenAsync(user, "Email", request.TwoFactorCode);
                    }

                    if (!twoFactorSuccess && validProviders.Contains("Authenticator"))
                    {
                        twoFactorSuccess = await userManager.VerifyTwoFactorTokenAsync(user, "Authenticator", request.TwoFactorCode);
                    }

                    if (!twoFactorSuccess)
                    {
                        return ResultHelper.CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken()));
                    }
                }
            }

            string accessToken = jwtTokenService.GenerateAccessToken(user);
            string refreshToken = jwtTokenService.GenerateRefreshToken(user);

            return TypedResults.Ok(new Response(accessToken, refreshToken));
        }).AllowAnonymous();
    }

    internal sealed record Request(string Email, string Password, string TwoFactorCode, string TwoFactorRecoveryCode);
    internal sealed record Response(string AccessToken, string RefreshToken);
}
