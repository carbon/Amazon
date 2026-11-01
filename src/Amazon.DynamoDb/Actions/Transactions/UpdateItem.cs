using Carbon.Data;
using Carbon.Data.Expressions;

namespace Amazon.DynamoDb.Transactions;

public sealed class UpdateItem
{
    public UpdateItem(
        string tableName,
        AttributeCollection key,
        Change[] changes,
        Expression[]? conditions = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(tableName);
        ArgumentNullException.ThrowIfNull(key);

        TableName = tableName;
        Key = key;

        var attributeNames = new Dictionary<string, string>();
        var attributeValues = new AttributeCollection();
        var updateExpression = new UpdateExpression(changes, attributeNames, attributeValues);

        UpdateExpression = updateExpression.ToString();

        if (conditions is { Length: > 0 })
        {
            var expression = new DynamoExpression(attributeNames, attributeValues);

            expression.AddRange(conditions);

            ConditionExpression = expression.Text;
        }

        if (attributeNames.Count > 0)
        {
            ExpressionAttributeNames = attributeNames;
        }

        if (attributeValues.Count > 0)
        {
            ExpressionAttributeValues = attributeValues;
        }
    }

    public required AttributeCollection Key { get; init; }

    public required string TableName { get; init; }

    public string UpdateExpression { get; init; }

    public string? ConditionExpression { get; init; }

    public Dictionary<string, string>? ExpressionAttributeNames { get; init; }

    public AttributeCollection? ExpressionAttributeValues { get; set; }

    public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }
}
