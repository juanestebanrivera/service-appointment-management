using Appointments.Application.Common.Interfaces;
using Appointments.Domain.SharedKernel;
using Appointments.Domain.Users;

namespace Appointments.Application.Features.Users.Commands.UserLogin;

public class UserLoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenGenerator tokenGenerator
)
: ICommandHandler<UserLoginCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenGenerator _tokenGenerator = tokenGenerator;

    public async Task<Result<AuthenticationResult>> HandleAsync(UserLoginCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is null or { IsActive: false })
            return Result<AuthenticationResult>.Failure(UserApplicationErrors.InvalidCredentials);

        var verified = _passwordHasher.Verify(command.Password, user.PasswordHash);

        if (!verified)
            return Result<AuthenticationResult>.Failure(UserApplicationErrors.InvalidCredentials);

        var accessToken = _tokenGenerator.GenerateToken(user);

        return Result<AuthenticationResult>.Success(new AuthenticationResult(accessToken));
    }
}