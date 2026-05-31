using Catalog.Domain.SharedKernel;

namespace Catalog.Api.Features.Services.V1.Requests;

public record GetServicesRequest(
    int Page = PageParameters.DefaultPage,
    int Size = PageParameters.DefaultPageSize,
    string? Search = null);
