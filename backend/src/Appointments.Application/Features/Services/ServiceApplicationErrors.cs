using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services;

public static class ServiceApplicationErrors
{
    public static readonly Error NotFound = new("Service.NotFound", "Service was not found.", ErrorType.NotFound);
}
