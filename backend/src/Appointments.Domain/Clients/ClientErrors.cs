using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Clients;

public static class ClientErrors
{
    public static readonly Error ClientIsInactive = new("Client.ClientIsInactive", "The client is inactive.", ErrorType.Conflict);
}