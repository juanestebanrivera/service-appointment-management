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

        var changeResult = client.ChangeName(command.FirstName, command.LastName);
        
        if (changeResult.IsFailure)
            return Result.Failure(changeResult.Error);        

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
