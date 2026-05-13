using Appointments.Application.Common.Pagination;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public record GetAllAppointmentsQuery(
    int Page = PaginationParams.DefaultPage,
    int PageSize = PaginationParams.DefaultPageSize,
    string? SearchTerm = null);