#nullable disable

namespace Amazon.DynamoDb;

public readonly struct ConsumedCapacity
{
    public string TableName { get; init; }

    public float CapacityUnits { get; init; }
}