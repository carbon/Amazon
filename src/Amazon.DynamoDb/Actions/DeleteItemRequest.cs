using Carbon.Data.Expressions;

namespace Amazon.DynamoDb;

public sealed class DeleteItemRequest
{
    public DeleteItemRequest(string tableName, IEnumerable<KeyValuePair<string, object>> key)
        : this(tableName, key.ToDictionary()) { }

    public DeleteItemRequest(string tableName, Dictionary<string, DbValue> key)
    {
        ArgumentException.ThrowIfNullOrEmpty(tableName);
        ArgumentNullException.ThrowIfNull(key);

        TableName = tableName;
        Key = key;
    }

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

    public string TableName { get; }

    public Dictionary<string, DbValue> Key { get; }

    public ReturnValues? ReturnValues { get; set; }

    public string? ConditionExpression { get; set; }

    public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

    public AttributeCollection? ExpressionAttributeValues { get; set; }
}

/*
{
	"TableName":"Table1",
    "Key": {"HashKeyElement":{"S":"AttributeValue1"},"RangeKeyElement":{"N":"AttributeValue2"}},
    "Expected":{"AttributeName3":{"Value":{"S":"AttributeValue3"}}},
    "ReturnValues":"ALL_OLD"}
}
*/
