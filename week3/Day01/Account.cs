public class Account
{
    private decimal balance;
    public Account(decimal balance) => this.balance = balance;

    public void Withdraw(decimal amount)
    {
        try
        {
            if (amount > balance)
                throw new InsufficientFundsException(amount - balance);
            balance -= amount;
            Console.WriteLine($"Withdrew {amount:C}. New balance: {balance:C}");
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Withdrawal failed: {ex.Message}");
        }
        finally
        {
            Console.WriteLine($"[Log] Withdrawal attempt of {amount:C} recorded.");
        }
    }
}