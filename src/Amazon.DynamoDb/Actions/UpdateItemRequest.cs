using System;
using System.Collections.Generic;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public sealed class UpdateItemRequest
    {
        public UpdateItemRequest(
            string tableName, 
            IEnumerable<KeyValuePair<string, object>> key,
            Change[] changes,
            Expression[]? conditions = null,
            ReturnValues? returnValues = null)
        {
            TableName    = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key          = key       ?? throw new ArgumentNullException(nameof(key));
            Changes      = changes   ?? throw new ArgumentNullException(nameof(changes));
            ReturnValues = returnValues;

            if (conditions != null && conditions.Length > 0)
            {
                var expression = new DynamoExpression(ExpressionAttributeNames, ExpressionAttributeValues);

                expression.AddRange(conditions);

                ConditionExpression = expression.Text;
            }
        }

        public string TableName { get; }

        public IEnumerable<KeyValuePair<string, object>> Key { get; }

        public Change[] Changes { get; }

        public string? ConditionExpression { get; }

        public JsonObject ExpressionAttributeNames { get; } = new JsonObject();

        public AttributeCollection ExpressionAttributeValues { get; } = new AttributeCollection();

        public ReturnValues? ReturnValues { get; }

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "TableName", TableName },
                { "Key", Key.ToJson() }
            };

            var updateExpression = new UpdateExpression(Changes, ExpressionAttributeNames, ExpressionAttributeValues);

            if (ConditionExpression != null)
            {
                json.Add("ConditionExpression", ConditionExpression);
            }

            if (ExpressionAttributeNames != null && ExpressionAttributeNames.Keys.Count > 0)
            {
                json.Add("ExpressionAttributeNames", ExpressionAttributeNames);
            }

            if (ExpressionAttributeValues.Count > 0)
            {
                json.Add("ExpressionAttributeValues", ExpressionAttributeValues.ToJson());
            }

            json.Add("UpdateExpression", updateExpression.ToString());

            if (ReturnValues != null)
            {
                json.Add("ReturnValues", ReturnValues.ToString()!);
            }

            return json;
        }
    }
}

/*
{	"TableName":"Table1",
    "Key": {"HashKeyElement":{"S":"AttributeValue1"},
        "RangeKeyElement":{"N":"AttributeValue2"}
	},
    "AttributeUpdates":{
		"AttributeName3":{"Value":{"S":"AttributeValue3_New"},"Action":"PUT"}
	},
    "Expected":{"AttributeName3":{"Value":{"S":"AttributeValue3_Current"}}},
    "ReturnValues":"ReturnValuesConstant"
}
*/
