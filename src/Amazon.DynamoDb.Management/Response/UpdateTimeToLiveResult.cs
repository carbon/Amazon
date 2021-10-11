#nullable disable

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb;

public sealed class UpdateTimeToLiveResult
{
    public TimeToLiveSpecification TimeToLiveSpecification { get; init; }
}