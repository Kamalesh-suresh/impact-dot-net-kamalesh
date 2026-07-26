using System;

public class BankAccount
{
	private decimal _balance;
	private readonly List<string> _history = new();

	public decimal Balance => _balance;


	public void Deposit(decimal amount)
	{
		if (amount <= 0)
		{
			throw new ArgumentException("Deposit must be positive");
		}
		_balance += amount;
		_history.Add($"Deposit  {amount:C}  →  {_balance:C}");
	}


	public void Withdraw(decimal amount) {

		if (amount <= 0)
		{
			throw new ArgumentException("Withdraw amount should be positive");
		}
		if (amount > _balance)
		{
			throw new InvalidOperationException("Insufficient Funds");
		}
		_balance -= amount;
		_history.Add($"Withdraw {amount:C}  →  {_balance:C}");


    }

	public void GetHistory()
	{
		foreach (var item in _history)
		{
			Console.WriteLine(item);
		}
	}
}

