namespace Amazon.DynamoDb.Models;

public sealed class BillingModeSummary
{
    public BillingMode BillingMode { get; init; }

    public Timestamp LastUpdateToPayPerRequestDateTime { get; init; }
}
