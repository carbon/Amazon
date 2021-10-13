namespace Amazon.DynamoDb;

public sealed class QueryResult
{
    public ConsumedCapacity? ConsumedCapacity { get; init; }

#nullable disable

    public AttributeCollection[] Items { get; init; }

    public Dictionary<string, DbValue> LastEvaluatedKey { get; init; }

    public int Count { get; init; }
}
