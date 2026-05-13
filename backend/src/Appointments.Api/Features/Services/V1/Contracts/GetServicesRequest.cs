using Appointments.Application.Common.Pagination;

namespace Appointments.Api.Features.Services.V1.Contracts;

public record GetServicesRequest(
    int Page = PaginationParams.DefaultPage,
    int Size = PaginationParams.DefaultPageSize,
    string? Search = null,
    bool? Status = null);
