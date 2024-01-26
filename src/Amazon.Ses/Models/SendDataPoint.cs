namespace Amazon.Ses;

public sealed class SendDataPoint
{
    public long Bounces { get; init; }

    public long Complaints { get; init; }

    public long DeliveryAttempts { get; init; }

    public long Rejects { get; init; }

    // Timestamp
}