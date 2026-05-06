using System.Runtime.Serialization;

namespace Appointments.Api.Features.Services.V1.Contracts;

[DataContract(Name = "Service")]
public record ServiceApiResponse(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration,
    bool IsActive
);