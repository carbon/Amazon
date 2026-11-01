namespace Amazon.Sns;

public sealed class UnsubscribeConfirmation : SnsMessage
{
    public required string SubscribeURL { get; init; }

    public required string Token { get; init; }
}