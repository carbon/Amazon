namespace Amazon.DynamoDb;

public sealed class QueryResult
{
    public ConsumedCapacity? ConsumedCapacity { get; init; }

    public required AttributeCollection[] Items { get; init; }

    public Dictionary<string, DbValue>? LastEvaluatedKey { get; init; }

    public int Count { get; init; }

    public long ScannedCount { get; init; }
}