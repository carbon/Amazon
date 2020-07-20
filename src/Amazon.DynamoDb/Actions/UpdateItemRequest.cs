using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.Json.Serialization;
using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public sealed class UpdateItemRequest
    {
        public UpdateItemRequest(
            string tableName, 
            Dictionary<string, DbValue> key,
            Change[] changes,
            Expression[]? conditions = null,
            ReturnValues? returnValues = null)
        {
            TableName    = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key          = key       ?? throw new ArgumentNullException(nameof(key));
            Changes      = changes   ?? throw new ArgumentNullException(nameof(changes));
            ReturnValues = returnValues;

            Dictionary<string, string> attrNames = new Dictionary<string, string>();
            var updateExpression = new UpdateExpression(Changes, attrNames, ExpressionAttributeValues);

            UpdateExpression = updateExpression.ToString();

            if (conditions != null && conditions.Length > 0)
            {
                var expression = new DynamoExpression(attrNames, ExpressionAttributeValues);

                expression.AddRange(conditions);

                ConditionExpression = expression.Text;
            }

            if (attrNames.Count > 0)
            {
                ExpressionAttributeNames = attrNames;
            }
        }

        public string TableName { get; }

        public Dictionary<string, DbValue> Key { get; }

        [JsonIgnore]
        public Change[] Changes { get; }

        public string? ConditionExpression { get; }

        public Dictionary<string, string> ExpressionAttributeNames { get; }

        public AttributeCollection ExpressionAttributeValues { get; } = new AttributeCollection();

        public ReturnValues? ReturnValues { get; }

        public string UpdateExpression { get; }
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
