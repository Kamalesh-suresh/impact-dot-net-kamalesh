public class InsufficientFundsException : Exception
{
    public decimal DeficitAmount { get; }

    public InsufficientFundsException(decimal deficitAmount)
        : base($"Insufficient funds. Short by {deficitAmount:C}.")
    {
        DeficitAmount = deficitAmount;
    }
}