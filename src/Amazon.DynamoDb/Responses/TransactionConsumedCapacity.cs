#nullable disable

namespace Amazon.DynamoDb;

public sealed class TransactionConsumedCapacity
{
    public float CapacityUnits { get; init; }

    public Dictionary<string, Capacity> GlobalSecondaryIndexes { get; init; }

    public Dictionary<string, Capacity> LocalSecondaryIndexes { get; init; }

    public float ReadCapacityUnits { get; init; }

    public Capacity Table { get; init; }

    public string TableName { get; init; }

    public float WriteCapacityUnits { get; init; }

    public sealed class Capacity
    {
        public float CapacityUnits { get; init; }

        public float ReadCapacityUnits { get; init; }

        public float WriteCapacityUnits { get; init; }
    }
}