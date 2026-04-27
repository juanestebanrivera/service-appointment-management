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
        var hashedPassword = _passwordHasher.Hash(command.Password);
        var user = await _userRepository.GetUserByEmailAndPasswordAsync(command.Email, hashedPassword, cancellationToken);

        if (user is null or { IsActive: false })
            return Result<AuthenticationResult>.Failure(UserApplicationErrors.InvalidCredentials);

        var accessToken = _tokenGenerator.GenerateToken(user);

        return Result<AuthenticationResult>.Success(new AuthenticationResult(accessToken));
    }
}