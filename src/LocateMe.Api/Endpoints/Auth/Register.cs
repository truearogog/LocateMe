using System.ComponentModel.DataAnnotations;
using LocateMe.Api.Helpers;
using LocateMe.Application.Abstractions.Events;
using LocateMe.Domain.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace LocateMe.Api.Endpoints.Auth;

internal sealed class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var emailAddressAttribute = new EmailAddressAttribute();

        app.MapPost("auth/register", async Task<Results<Ok, ValidationProblem>> (
            Request request,
            IEventBus eventBus,
            UserManager<User> userManager,
            IUserStore<User> userStore,
            CancellationToken cancellationToken) =>
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException($"{nameof(Register)} requires a user store with email support.");
            }

            var emailStore = (IUserEmailStore<User>)userStore;

            if (string.IsNullOrEmpty(request.Email) || !emailAddressAttribute.IsValid(request.Email))
            {
                return ResultHelper.CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(request.Email)));
            }

            var user = new User();
            await userStore.SetUserNameAsync(user, request.Email, cancellationToken);
            await emailStore.SetEmailAsync(user, request.Email, cancellationToken);
            IdentityResult result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return ResultHelper.CreateValidationProblem(result);
            }

            await eventBus.PublishAsync(new UserRegisteredDomainEvent(Guid.NewGuid(), user), cancellationToken);

            return TypedResults.Ok();
        }).AllowAnonymous();
    }

    internal sealed record Request(string Email, string Password);
}
