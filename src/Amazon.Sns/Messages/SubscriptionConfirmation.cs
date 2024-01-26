namespace Amazon.Sns;

public sealed class SubscriptionConfirmation : SnsMessage
{
    public required string SubscribeURL { get; init; }
}