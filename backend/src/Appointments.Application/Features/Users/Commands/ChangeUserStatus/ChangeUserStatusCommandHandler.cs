using Appointments.Application.Common.Interfaces;
using Appointments.Domain.SharedKernel;
using Appointments.Domain.Users;

namespace Appointments.Application.Features.Users.Commands.ChangeUserStatus;

public class ChangeUserStatusCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<ChangeUserStatusCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(ChangeUserStatusCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
            return Result.Failure(UserApplicationErrors.UserNotFound);

        if (command.IsActive)
        {
            user.Activate();
        }
        else
        {
            user.Desactivate();
        }

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}