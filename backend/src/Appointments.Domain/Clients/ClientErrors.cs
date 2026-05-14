using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Clients;

public static class ClientErrors
{
    public static readonly Error ClientIsInactive = new(ErrorType.Conflict, "The client is inactive.");
}