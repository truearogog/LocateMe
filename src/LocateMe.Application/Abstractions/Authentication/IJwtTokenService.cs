using System.Security.Claims;
using LocateMe.Domain.Users;

namespace LocateMe.Application.Abstractions.Authentication;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    ClaimsPrincipal? ValidateRefreshToken(string refreshToken);
}
