using Shared.Contracts.DDD;

namespace Shared.Contracts.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public Money(decimal amount, string currency)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currency);

        Amount = Math.Round(amount, 4);
        Currency = currency;
    }

    public static Money Rupiah(decimal amount)
    {
        return new Money(amount, "IDR");
    }
};