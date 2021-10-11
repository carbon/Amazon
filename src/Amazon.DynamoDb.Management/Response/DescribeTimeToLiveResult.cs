#nullable disable

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb;

public sealed class DescribeTimeToLiveResult
{
    public TimeToLiveDescription TimeToLiveDescription { get; init; }
}