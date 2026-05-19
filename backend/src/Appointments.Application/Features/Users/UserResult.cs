using Appointments.Domain.Users;

namespace Appointments.Application.Features.Users;

public record UserResult(
    Guid Id,
    string Email,
    UserRole Role,
    bool IsActive,
    Guid? ClientId,
    string? ClientFirstName,
    string? ClientLastName
);
