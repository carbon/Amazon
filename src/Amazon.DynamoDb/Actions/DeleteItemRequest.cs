using System;
using System.Collections.Generic;

using Carbon.Data.Expressions;

namespace Amazon.DynamoDb
{
    public sealed class DeleteItemRequest
    {
        public DeleteItemRequest(string tableName, IEnumerable<KeyValuePair<string, object>> key)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
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

        public IEnumerable<KeyValuePair<string, object>> Key { get; }

        public ReturnValues ReturnValues { get; set; }

        public string? ConditionExpression { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public AttributeCollection? ExpressionAttributeValues { get; set; }
    }
}

/*
{
	"TableName":"Table1",
    "Key": {"HashKeyElement":{"S":"AttributeValue1"},"RangeKeyElement":{"N":"AttributeValue2"}},
    "Expected":{"AttributeName3":{"Value":{"S":"AttributeValue3"}}},
    "ReturnValues":"ALL_OLD"}
}
*/
