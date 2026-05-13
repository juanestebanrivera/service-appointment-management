using Appointments.Application.Common.Pagination;

namespace Appointments.Api.Features.Clients.V1.Contracts;

public record GetAllClientsRequest(
    int Page = PaginationParams.DefaultPage,
    int Size = PaginationParams.DefaultPageSize,
    string? Search = null,
    bool? Status = null);
