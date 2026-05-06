using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Users;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;
using Appointments.Domain.SharedKernel.ValueObjects;
using Appointments.Domain.Users;

namespace Appointments.Application.Features.Clients.Commands.CreateClient;

public sealed class CreateClientCommandHandler(
    IClientRepository clientRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateClientCommand, Guid>
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUserRepository _userRepository = userRepository;
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

        var resultFirstName = PersonName.Create(command.FirstName, nameof(Client.FirstName));

        if (resultFirstName.IsFailure)
            return Result<Guid>.Failure(resultFirstName.Error);

        var resultLastName = PersonName.Create(command.LastName, nameof(Client.LastName));

        if (resultLastName.IsFailure)
            return Result<Guid>.Failure(resultLastName.Error);

        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);

        if (user is null or { IsActive: false })
            return Result<Guid>.Failure(UserApplicationErrors.UserNotFound);

        var clientResult = Client.Register(resultFirstName.Value, resultLastName.Value, phoneResult.Value, user.Id, email);

        if (clientResult.IsFailure)
            return Result<Guid>.Failure(clientResult.Error);

        _clientRepository.Add(clientResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(clientResult.Value.Id);
    }
}