using System;

public class Notification
{
	public virtual void Send()
	{
        Console.WriteLine("Sent");
	}
}


public class EmailNotification : Notification
{
	public sealed override void Send()
	{
		Console.WriteLine("Sent by email");
	}
}


public class SMSNotification : Notification
{
    public  override void Send()
    {
        Console.WriteLine("Sent by SMS");
    }
}

public class PriorityNotification : EmailNotification
{
    public override void Send()
    {
        Console.WriteLine("Sent by Priority email");
    }
}
