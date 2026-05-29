using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Services;

public static class MoneyErrors
{
    public static readonly Error InvalidAmount = new(ErrorType.Validation, "Amount must be a non-negative value.");
    public static readonly Error InvalidCurrency = new(ErrorType.Validation, "Currency must be COP or USD.");
}