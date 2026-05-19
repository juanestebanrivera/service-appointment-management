using Appointments.Domain.Users;

namespace Appointments.Application.Features.Users;

public record UserResult(
    Guid Id,
    Guid? ClientId,
    string Email,
    UserRole Role,
    bool IsActive
);
