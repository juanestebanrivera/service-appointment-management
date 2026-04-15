using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Commands.UpdateClient;

public sealed class UpdateClientCommandHandler(
    IClientRepository clientRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateClientCommand>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(UpdateClientCommand command, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId, cancellationToken);

        if (client is null)
            return Result.Failure(ClientApplicationErrors.NotFound);

        var resultFirstName = PersonName.Create(command.FirstName, nameof(Client.FirstName));

        if (resultFirstName.IsFailure)
            return Result.Failure(resultFirstName.Error);

        var resultLastName = PersonName.Create(command.LastName, nameof(Client.LastName));

        if (resultLastName.IsFailure)
            return Result.Failure(resultLastName.Error);

        client.ChangeName(resultFirstName.Value, resultLastName.Value);

        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var emailResult = Email.Create(command.Email);

            if (emailResult.IsFailure)
                return Result.Failure(emailResult.Error);

            client.ChangeEmail(emailResult.Value);
        }
        else
        {
            client.ChangeEmail(null);
        }

        var phoneResult = PhoneNumber.Create(command.PhonePrefix, command.PhoneNumber);

        if (phoneResult.IsFailure)
            return Result.Failure(phoneResult.Error);

        client.ChangePhoneNumber(phoneResult.Value);

        if (command.IsActive)
            client.Activate();
        else
            client.Deactivate();

        _clientRepository.Update(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
