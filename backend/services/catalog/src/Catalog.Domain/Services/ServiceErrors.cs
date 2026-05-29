using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Services;

public static class ServiceErrors
{
    public static readonly Error NameCannotBeEmpty = new(ErrorType.Validation, "Service name cannot be empty.");
    public static readonly Error NameMustBeUnique = new(ErrorType.Validation, "A service with the same name already exists.");
    public static readonly Error PriceIsRequired = new(ErrorType.Validation, "Service price is required.");
    public static readonly Error DurationIsRequired = new(ErrorType.Validation, "Service duration is required.");
    public static readonly Error NotFound = new(ErrorType.NotFound, "Service not found.");
}