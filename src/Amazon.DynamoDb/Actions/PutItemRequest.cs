using System;

using Carbon.Data.Expressions;
using Carbon.Json;

namespace Amazon.DynamoDb
{
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

        public JsonObject? ExpressionAttributeNames { get; set; }

        public AttributeCollection? ExpressionAttributeValues { get; set; }

        public ReturnValues ReturnValues { get; set; }

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

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "TableName" , TableName },
                { "Item", Item.ToJson() }
            };

            if (ConditionExpression != null)
            {
                json.Add("ConditionExpression", ConditionExpression);
            }

            if (ExpressionAttributeNames != null && ExpressionAttributeNames.Keys.Count > 0)
            {
                json.Add("ExpressionAttributeNames", ExpressionAttributeNames);
            }

            if (ExpressionAttributeValues != null && ExpressionAttributeValues.Count > 0)
            {
                json.Add("ExpressionAttributeValues", ExpressionAttributeValues.ToJson());
            }

            if (ReturnValues != ReturnValues.NONE)
            {
                json.Add("ReturnValues", ReturnValues.Canonicalize());
            }

            return json;
        }
    }
}
