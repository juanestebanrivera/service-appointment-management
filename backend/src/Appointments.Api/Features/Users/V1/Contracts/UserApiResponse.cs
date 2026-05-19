namespace Appointments.Api.Features.Users.V1.Contracts;

public record UserApiResponse(
    Guid Id,
    Guid? ClientId,
    string? Name,
    string Email,
    string Role,
    bool IsActive
);
