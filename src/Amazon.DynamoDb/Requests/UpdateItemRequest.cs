using System;
using System.Collections.Generic;

using Carbon.Data;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class UpdateItemRequest
    {
        public UpdateItemRequest(string tableName, RecordKey key, IList<Change> changes)
        {
            #region Preconditions

            if (tableName == null)
                throw new ArgumentNullException(nameof(TableName));

            if (changes == null)
                throw new ArgumentNullException(nameof(changes));
           
            #endregion

            TableName = tableName;
            Key = key;
            Changes = changes;
        }

        public string TableName { get; }

        public RecordKey Key { get; }

        public IList<Change> Changes { get; }

        public string ConditionExpression { get; set; }

        public JsonObject ExpressionAttributeNames { get; } = new JsonObject();

        public AttributeCollection ExpressionAttributeValues { get; } = new AttributeCollection();

        public ReturnValues? ReturnValues { get; set; }

        internal void SetConditions(Expression[] conditions)
        {
            var expression = new DynamoExpression(ExpressionAttributeNames, ExpressionAttributeValues);

            expression.AddRange(conditions);

            ConditionExpression = expression.Text;
        }

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "TableName", TableName },
                { "Key", Key.ToJson() }
            };

            var updateExpression = new UpdateExpression(Changes, ExpressionAttributeNames, ExpressionAttributeValues);


            if (ConditionExpression != null) json.Add("ConditionExpression", ConditionExpression);

            if (ExpressionAttributeNames != null && ExpressionAttributeNames.Keys.Count > 0)
            {
                json.Add("ExpressionAttributeNames", ExpressionAttributeNames);
            }

            if (ExpressionAttributeValues.Count > 0)
            {
                json.Add("ExpressionAttributeValues", ExpressionAttributeValues.ToJson());
            }

            json.Add("UpdateExpression", updateExpression.ToString());

            if (ReturnValues != null) json.Add("ReturnValues", ReturnValues.ToString());

            return json;
        }
    }

    public enum ReturnValues
    {
        /// <summary>
        /// If this parameter is not provided or is NONE, nothing is returned. 
        /// </summary>
        NONE = 0,

        /// <summary>
        /// ALL_OLD is specified, and UpdateItem overwrote an attribute name-value pair, the content of the old item is returned. 
        /// </summary>
        ALL_OLD,

        UPDATED_OLD,

        /// <summary>
        /// All the attributes of the new version of the item are returned. 
        /// </summary>
        ALL_NEW,

        /// <summary>
        /// The new versions of only the updated attributes are returned.
        /// </summary>
        UPDATED_NEW
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
