using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Clients.Queries;
using Appointments.Domain.SharedKernel;
using Appointments.Domain.Users;

namespace Appointments.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    IClientQueryRepository clientQueryRepository
) : IQueryHandler<GetUserByIdQuery, UserResult>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IClientQueryRepository _clientQueryRepository = clientQueryRepository;

    public async Task<Result<UserResult>> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        if (!query.IsAdmin && query.UserId != query.CurrentUserId)
            return Result<UserResult>.Failure(UserApplicationErrors.Forbidden);

        var user = await _userRepository.GetByIdAsync(query.UserId, cancellationToken);

        if (user is null)
            return Result<UserResult>.Failure(UserApplicationErrors.UserNotFound);

        var client = await _clientQueryRepository.GetByUserIdAsync(query.UserId, cancellationToken);

        return Result<UserResult>.Success(new UserResult(
            user.Id,
            user.Email.Value,
            user.Role,
            user.IsActive,
            client?.Id,
            client?.FirstName?.Value,
            client?.LastName?.Value
        ));
    }
}
