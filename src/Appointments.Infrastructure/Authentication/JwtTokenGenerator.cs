using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Users;
using Microsoft.IdentityModel.Tokens;

namespace Appointments.Infrastructure.Authentication;

public class JwtTokenGenerator(JwtConfiguration jwtConfiguration) : ITokenGenerator
{
    private readonly JwtConfiguration _configuration = jwtConfiguration;

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("role", user.Role.ToString())
        ];

        var token = new JwtSecurityToken(
            _configuration.Issuer,
            _configuration.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}