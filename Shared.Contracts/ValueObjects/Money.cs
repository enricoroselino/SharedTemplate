using Shared.Contracts.DDD;

namespace Shared.Contracts.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public Money(decimal amount, string currency)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currency);

        Amount = amount;
        Currency = currency;
    }

    public static Money Rupiah(decimal amount)
    {
        return new Money(amount, "IDR");
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }
};