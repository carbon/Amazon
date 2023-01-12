using Carbon.Data;
using Carbon.Data.Expressions;

namespace Amazon.DynamoDb;

public sealed class UpdateItemRequest
{
    public UpdateItemRequest(
        string tableName,
        IReadOnlyDictionary<string, DbValue> key,
        Change[] changes,
        Expression[]? conditions = null,
        ReturnValues? returnValues = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(tableName);
        ArgumentNullException.ThrowIfNull(key);

        TableName = tableName;
        Key = key;
        ReturnValues = returnValues;

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

    public string TableName { get; }

    public IReadOnlyDictionary<string, DbValue> Key { get; }

    public string? ConditionExpression { get; }

    public IReadOnlyDictionary<string, string>? ExpressionAttributeNames { get; }

    public AttributeCollection? ExpressionAttributeValues { get; }

    public ReturnValues? ReturnValues { get; }

    public string UpdateExpression { get; }
}