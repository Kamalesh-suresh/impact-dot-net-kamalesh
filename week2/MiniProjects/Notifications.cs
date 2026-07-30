using System;


// ── Q5 · Notification Engine ──────────────────────────────────

public interface INotificationSender
{
    void Send(string message);
}

public class EmailSender : INotificationSender
{
    public void Send(string m) => Console.WriteLine($"EMAIL: {m}");
}

public class SmsSender : INotificationSender
{
    public void Send(string m) => Console.WriteLine($"SMS: {m}");
}

public class PushSender : INotificationSender
{
    public void Send(string m) => Console.WriteLine($"PUSH: {m}");
}

public class NotificationService
{
    // an event, not a raw delegate: outsiders may only += / -=,
    // they can't invoke it or wipe other subscribers
    public event EventHandler<string>? OnNotificationSent;

    public void Notify(INotificationSender sender, string message)
    {
        sender.Send(message);
        OnNotificationSent?.Invoke(this, message);   // ?. = no subscribers, no crash
    }
}
