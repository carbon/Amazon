namespace Amazon.DynamoDb.Transactions;

public sealed class PutItem
{
    public PutItem(
        string tableName,
        AttributeCollection item)
    {
        ArgumentNullException.ThrowIfNull(tableName);
        ArgumentNullException.ThrowIfNull(item);

        TableName = tableName;
        Item = item;
    }

    public required AttributeCollection Item { get; init; }

    public required string TableName { get; init; }

    public string? ConditionExpression { get; init; }

    public Dictionary<string, string>? ExpressionAttributeNames { get; init; }

    public AttributeCollection? ExpressionAttributeValues { get; init; }

    public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; init; }
}