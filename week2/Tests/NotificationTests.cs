using Xunit;

public class SpySender : INotificationSender
{
    public string? LastMessage { get; private set; }
    public int SendCount { get; private set; }

    public void Send(string message)
    {
        LastMessage = message;
        SendCount++;
    }
}

public class NotificationTests
{
    [Fact]
    public void Notify_CallsSenderSend_WithTheMessage()
    {
        var svc = new NotificationService();
        var sender = new SpySender();

        svc.Notify(sender, "Order shipped");

        Assert.Equal(1, sender.SendCount);
        Assert.Equal("Order shipped", sender.LastMessage);
    }

    [Fact]
    public void Notify_FiresOnNotificationSent_ForSingleSubscriber()
    {
        var svc = new NotificationService();
        string? received = null;
        svc.OnNotificationSent += (s, msg) => received = msg;

        svc.Notify(new SpySender(), "Payment received");

        Assert.Equal("Payment received", received);
    }

    [Fact]
    public void Notify_FiresOnNotificationSent_ForMultipleSubscribers()
    {
        var svc = new NotificationService();
        var log = new System.Collections.Generic.List<string>();
        svc.OnNotificationSent += (s, msg) => log.Add($"A:{msg}");
        svc.OnNotificationSent += (s, msg) => log.Add($"B:{msg}");

        svc.Notify(new SpySender(), "New login detected");

        Assert.Equal(2, log.Count);
        Assert.Contains("A:New login detected", log);
        Assert.Contains("B:New login detected", log);
    }

    [Fact]
    public void Notify_WithNoSubscribers_DoesNotThrow()
    {
        var svc = new NotificationService();
        var sender = new SpySender();

        var ex = Record.Exception(() => svc.Notify(sender, "no subscribers here"));

        Assert.Null(ex);
        Assert.Equal(1, sender.SendCount);
    }

    [Fact]
    public void Notify_WorksWithRealSenders_EmailSmsPush()
    {
        var svc = new NotificationService();
        int fired = 0;
        svc.OnNotificationSent += (s, msg) => fired++;

        svc.Notify(new EmailSender(), "m1");
        svc.Notify(new SmsSender(), "m2");
        svc.Notify(new PushSender(), "m3");

        Assert.Equal(3, fired);
    }
}
