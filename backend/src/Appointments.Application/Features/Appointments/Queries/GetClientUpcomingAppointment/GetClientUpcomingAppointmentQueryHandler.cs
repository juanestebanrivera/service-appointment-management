using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Clients;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetClientUpcomingAppointment;

public sealed class GetClientUpcomingAppointmentQueryHandler(IAppointmentQueryRepository appointmentRepository, IClientRepository clientRepository)
    : IQueryHandler<GetClientUpcomingAppointmentQuery, ClientUpcomingAppointmentsResult>
{
    private readonly IAppointmentQueryRepository _appointmentRepository = appointmentRepository;
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<Result<ClientUpcomingAppointmentsResult>> HandleAsync(GetClientUpcomingAppointmentQuery query, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(query.ClientId, cancellationToken);

        if (client is null)
            return Result<ClientUpcomingAppointmentsResult>.Failure(ClientApplicationErrors.NotFound);

        if (!query.IsAdmin && client.UserId != query.CurrentUserId)
            return Result<ClientUpcomingAppointmentsResult>.Failure(ClientApplicationErrors.Forbidden);

        var nextAppointment = await _appointmentRepository.GetClientUpcomingAppointmentAsync(query.ClientId, cancellationToken);

        ClientAppointmentResult? lastAppointment = null;

        if (query.IncludeLast)
        {
            lastAppointment = await _appointmentRepository.GetClientLastCompletedAppointmentAsync(query.ClientId, cancellationToken);
        }

        return Result<ClientUpcomingAppointmentsResult>.Success(new ClientUpcomingAppointmentsResult(nextAppointment, lastAppointment));
    }
}
