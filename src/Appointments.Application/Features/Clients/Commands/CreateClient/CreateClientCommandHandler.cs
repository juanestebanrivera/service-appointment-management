using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Commands.CreateClient;

public sealed class CreateClientCommandHandler(
    IClientRepository clientRepository,
    IUnitOfWork unitOfWork
) : ICreateClientCommandHandler
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> HandleAsync(CreateClientCommand command, CancellationToken cancellationToken = default)
    {
        var phoneResult = PhoneNumber.Create(command.PhonePrefix, command.PhoneNumber);

        if (phoneResult.IsFailure)
            return Result<Guid>.Failure(phoneResult.Error);

        Email? email = null;

        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var emailResult = Email.Create(command.Email);

            if (emailResult.IsFailure)
                return Result<Guid>.Failure(emailResult.Error);

            email = emailResult.Value;
        }

        var clientResult = Client.Register(command.FirstName, command.LastName, phoneResult.Value, email);

        if (clientResult.IsFailure)
            return Result<Guid>.Failure(clientResult.Error);

        _clientRepository.Add(clientResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(clientResult.Value.Id);
    }
}