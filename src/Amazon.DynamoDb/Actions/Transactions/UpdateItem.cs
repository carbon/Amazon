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
        TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        Key = key ?? throw new ArgumentNullException(nameof(key));

        var attributeNames = new Dictionary<string, string>();
        var updateExpression = new UpdateExpression(changes, attributeNames, ExpressionAttributeValues);

        UpdateExpression = updateExpression.ToString();

        if (conditions is { Length: > 0 })
        {
            var expression = new DynamoExpression(attributeNames, ExpressionAttributeValues);

            expression.AddRange(conditions);

            ConditionExpression = expression.Text;
        }

        if (attributeNames.Count > 0)
        {
            ExpressionAttributeNames = attributeNames;
        }
    }

    public AttributeCollection Key { get; init; }

    public string TableName { get; init; }

    public string UpdateExpression { get; init; }

    public string? ConditionExpression { get; init; }

    public Dictionary<string, string>? ExpressionAttributeNames { get; init; }

    public AttributeCollection ExpressionAttributeValues { get; set; } = new();

    public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }
}
