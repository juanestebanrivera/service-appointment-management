using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients;

public static class ClientApplicationErrors
{
    public static readonly Error NotFound = new(ErrorType.NotFound, "Client was not found.");
}
