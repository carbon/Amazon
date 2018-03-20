using System;
using System.Collections.Generic;

using Carbon.Data.Expressions;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class DeleteItemRequest
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

        public string ConditionExpression { get; set; }

        public JsonObject ExpressionAttributeNames { get; set; }

        public AttributeCollection ExpressionAttributeValues { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "TableName",  TableName },
                { "Key",        Key.ToJson() }
            };

            if (ConditionExpression != null)        json.Add("ConditionExpression", ConditionExpression);
            if (ExpressionAttributeNames != null)   json.Add("ExpressionAttributeNames", ExpressionAttributeNames);
            if (ExpressionAttributeValues != null)  json.Add("ExpressionAttributeValues", ExpressionAttributeValues.ToJson());
            if (ReturnValues != ReturnValues.NONE)  json.Add("ReturnValues", ReturnValues.ToString());

            return json;
        }
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
