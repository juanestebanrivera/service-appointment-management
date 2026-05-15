using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients;

public static class ClientApplicationErrors
{
    public static readonly Error NotFound = new(ErrorType.NotFound, "Client was not found.");
    public static readonly Error Forbidden = new(ErrorType.Forbidden, "You don't have permission to access this resource.");

    // The following errors have generic format for security reasons to avoid exposing data existence.
    public static readonly Error PhoneAlreadyInUse = new(ErrorType.Validation, "The phone number is invalid. Please provide a different phone number.");
    public static readonly Error EmailAlreadyInUse = new(ErrorType.Validation, "The email address is invalid. Please provide a different email address.");
    public static readonly Error HasAppointments = new(ErrorType.Conflict, "The client cannot be deleted because they have associated appointments.");
}
