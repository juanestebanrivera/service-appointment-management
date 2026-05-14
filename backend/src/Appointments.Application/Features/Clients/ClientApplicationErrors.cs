using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients;

public static class ClientApplicationErrors
{
    public static readonly Error NotFound = new(ErrorType.NotFound, "Client was not found.");
    public static readonly Error Forbidden = new(ErrorType.Forbidden, "You don't have permission to access this resource.");
}
