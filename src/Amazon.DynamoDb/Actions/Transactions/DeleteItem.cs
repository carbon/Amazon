namespace Amazon.DynamoDb.Transactions;

public sealed class DeleteItem
{
    public DeleteItem(string tableName, AttributeCollection key)
    {
        ArgumentNullException.ThrowIfNull(tableName);
        ArgumentNullException.ThrowIfNull(key);

        TableName = tableName;
        Key = key;
    }

    public AttributeCollection Key { get; init; }

    public string TableName { get; init; }

    public string? ConditionExpression { get; init; }

    public Dictionary<string, string>? ExpressionAttributeNames { get; init; }

    public AttributeCollection? ExpressionAttributeValues { get; init; }

    public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; init; }
}
