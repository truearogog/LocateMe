using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LocateMe.Application.Abstractions.Authentication;
using LocateMe.Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LocateMe.Infrastructure.Authentication;

internal sealed class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    private readonly string Key = configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key string is null");
    private readonly string Issuer = configuration["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer string is null");
    private readonly string Audience = configuration["Jwt:Audience"] ?? throw new Exception("Jwt:Audience string is null");

    public string GenerateAccessToken(User user)
    {
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),  // Short expiry for access token
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken(User user)
    {
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var refreshToken = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(7),  // Longer expiry for refresh token
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(refreshToken);
    }

    public ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true, // Allow token expiration check
                ValidateIssuerSigningKey = true,
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
            };

            ClaimsPrincipal principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);

            // Ensure the token has not expired
            if (validatedToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        catch (Exception)
        {
            // Log or handle token validation failure as needed
            return null;
        }
    }
}
