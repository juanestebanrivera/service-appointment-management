using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Appointments.Domain.Users;

namespace Appointments.Api.Shared.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        return Guid.TryParse(value, out var id) ? id : Guid.Empty;
    }

    public static bool IsAdmin(this ClaimsPrincipal user) =>
        user.IsInRole(nameof(UserRole.Admin));
}
