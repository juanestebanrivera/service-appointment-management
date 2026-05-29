using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Services;

public record Money
{
    private static readonly string[] AllowedCurrencies = ["COP", "USD"];

    public decimal Amount { get; init; }
    public string Currency { get; init; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Result<Money> Create(decimal amount, string currency)
    {
        if (amount < 0)
            return Result<Money>.Failure(MoneyErrors.InvalidAmount);

        if (string.IsNullOrWhiteSpace(currency) || !AllowedCurrencies.Contains(currency.ToUpper()))
            return Result<Money>.Failure(MoneyErrors.InvalidCurrency);

        return Result<Money>.Success(new Money(amount, currency));
    }
}