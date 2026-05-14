using Appointments.Application.Common.Pagination;

namespace Appointments.Api.Features.Clients.V1.Contracts;

public record GetClientAppointmentsRequest(
    int Page = PaginationParams.DefaultPage,
    int Size = PaginationParams.DefaultPageSize
);
