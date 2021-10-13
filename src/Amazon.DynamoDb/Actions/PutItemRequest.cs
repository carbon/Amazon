
using Carbon.Data.Expressions;

namespace Amazon.DynamoDb;

public sealed class PutItemRequest
{
    public PutItemRequest(string tableName, AttributeCollection item)
    {
        TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        Item = item ?? throw new ArgumentNullException(nameof(item));
    }

    public string TableName { get; }

    public AttributeCollection Item { get; }

    public string? ConditionExpression { get; set; }

    public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

    public AttributeCollection? ExpressionAttributeValues { get; set; }

    public ReturnValues? ReturnValues { get; set; }

    internal void SetConditions(Expression[] conditions)
    {
        var expression = DynamoExpression.Conjunction(conditions);

        ConditionExpression = expression.Text;

        if (expression.HasAttributeNames)
        {
            ExpressionAttributeNames = expression.AttributeNames;
        }

        if (expression.HasAttributeValues)
        {
            ExpressionAttributeValues = expression.AttributeValues;
        }
    }
}
