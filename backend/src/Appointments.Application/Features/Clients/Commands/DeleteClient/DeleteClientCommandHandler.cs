using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Commands.DeleteClient;

public sealed class DeleteClientCommandHandler(
    IClientRepository clientRepository,
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteClientCommand>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(DeleteClientCommand command, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId, cancellationToken);

        if (client is null)
            return Result.Failure(ClientApplicationErrors.NotFound);

        if (await _appointmentRepository.ExistsByClientAsync(client.Id, cancellationToken))
            return Result.Failure(ClientApplicationErrors.HasAppointments);

        _clientRepository.Delete(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
