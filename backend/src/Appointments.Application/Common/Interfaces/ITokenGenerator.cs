using Appointments.Domain.Users;

namespace Appointments.Application.Common.Interfaces;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}