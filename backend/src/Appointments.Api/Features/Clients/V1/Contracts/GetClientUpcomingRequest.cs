namespace Appointments.Api.Features.Clients.V1.Contracts;

public record GetClientUpcomingRequest(bool IncludeLast = false);
