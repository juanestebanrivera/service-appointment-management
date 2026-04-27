using Appointments.Application.Common.Interfaces;
using Appointments.Domain.SharedKernel;
using Appointments.Domain.SharedKernel.ValueObjects;
using Appointments.Domain.Users;

namespace Appointments.Application.Features.Users.Commands.UserRegister;

public class UserRegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork
)
: ICommandHandler<UserRegisterCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(UserRegisterCommand command, CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
            return Result.Failure(emailResult.Error);

        var existUserWithEmail = await _userRepository.VerifyIfEmailExistsAsync(emailResult.Value.Value, cancellationToken);

        if (existUserWithEmail)
            return Result.Failure(UserApplicationErrors.InvalidEmail);

        var userResult = User.Register(emailResult.Value, command.Password, _passwordHasher);

        if (userResult.IsFailure)
            return Result.Failure(userResult.Error);

        _userRepository.Add(userResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}