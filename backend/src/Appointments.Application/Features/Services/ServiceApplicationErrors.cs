using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services;

public static class ServiceApplicationErrors
{
    public static readonly Error NotFound = new(ErrorType.NotFound, "Service was not found.");
    public static readonly Error HasAppointments = new(ErrorType.Conflict, "The service cannot be deleted because it has associated appointments.");
}
