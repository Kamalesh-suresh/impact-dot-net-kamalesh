readonly struct Money
{
    public decimal Amount { get; }
    public string Currency { get; }
    public Money(decimal amount,string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money operator +(Money a,Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Currency mismatch");

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static bool operator ==(Money a, Money b) => a.Amount == b.Amount && a.Currency == b.Currency;
    public static bool operator !=(Money a, Money b) => !(a == b);

    public static bool operator >(Money a, Money b) => SameCcy(a, b) && a.Amount > b.Amount;
    public static bool operator <(Money a, Money b) => SameCcy(a, b) && a.Amount < b.Amount;
    private static bool SameCcy(Money a, Money b) =>
        a.Currency == b.Currency ? true : throw new InvalidOperationException("Currency mismatch");

    public override bool Equals(object? o) => o is Money m && this == m;
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Amount:0.00} {Currency}";
}


