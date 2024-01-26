namespace Amazon.Sns;

public sealed class Notification : SnsMessage
{
    public string? UnsubscribeURL { get; set; }
}

// https://docs.aws.amazon.com/ses/latest/dg/configure-sns-notifications.html