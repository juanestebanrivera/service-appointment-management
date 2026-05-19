using Appointments.Api.Features.Users.V1.Contracts;
using Appointments.Application.Features.Users;

namespace Appointments.Api.Features.Users.V1;

public static class UserApiMappings
{
    public static UserApiResponse ToUserApiResponse(this UserResult result)
    {
        return new UserApiResponse(
            result.Id,
            result.ClientId,
            $"{result.ClientFirstName} {result.ClientLastName}".Trim(),
            result.Email,
            result.Role.ToString(),
            result.IsActive
        );
    }
}