namespace Amazon.DynamoDb.Models;

public sealed class TimeToLiveDescription
{
    public string? AttributeName { get; init; }

    public TimeToLiveStatus TimeToLiveStatus { get; init; }
}
