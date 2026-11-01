namespace Amazon.Sns;

public sealed class ConfirmSubscriptionResult(string subscriptionArn)
{
    public string SubscriptionArn { get; } = subscriptionArn;
}
