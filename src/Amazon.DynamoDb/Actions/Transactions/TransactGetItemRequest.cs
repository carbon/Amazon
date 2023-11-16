namespace Amazon.DynamoDb.Transactions;

public sealed class TransactGetItemRequest
{
    public TransactGetItemRequest(TransactGetItem[] transactItems)
    {
        ArgumentNullException.ThrowIfNull(transactItems);

        TransactItems = transactItems;
    }

    public TransactGetItem[] TransactItems { get; init; }

    public ReturnConsumedCapacity? ReturnConsumedCapacity { get; init; }
}

public sealed class TransactGetItem(Get get)
{
    public Get Get { get; } = get ?? throw new ArgumentNullException(nameof(get));
}

public sealed class Get
{
    public Get(
        string tableName,
        IReadOnlyDictionary<string, DbValue> key)
    {
        ArgumentException.ThrowIfNullOrEmpty(tableName);
        ArgumentNullException.ThrowIfNull(key);

        TableName = tableName;
        Key = key;
    }

    public required string TableName { get; init; }

    public required IReadOnlyDictionary<string, DbValue> Key { get; init; }

    public Dictionary<string, string>? ExpressionAttributeNames { get; init; }

    public string? ProjectionExpression { get; init; }
}