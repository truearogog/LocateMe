using LocateMe.Application.Abstractions.Events;
using LocateMe.Domain.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace LocateMe.Api.Endpoints.Auth;

internal sealed class ResendConfirmationEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/resend-confirmation-email", async Task<Results<Ok, BadRequest<string>>> (
            Request request,
            IEventBus eventBus,
            UserManager<User> userManager,
            CancellationToken cancellationToken) =>
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return TypedResults.BadRequest("Email is required.");
            }

            User user = await userManager.FindByEmailAsync(request.Email);
            if (user is null || await userManager.IsEmailConfirmedAsync(user))
            {
                return TypedResults.Ok();
            }

            await eventBus.PublishAsync(new UserEmailConfirmationRequestedDomainEvent(Guid.NewGuid(), user), cancellationToken);
            return TypedResults.Ok();
        }).AllowAnonymous();
    }

    internal sealed record Request(string Email);
}
