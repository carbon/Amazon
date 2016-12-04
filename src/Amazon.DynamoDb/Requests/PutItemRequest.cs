using System;

using Carbon.Data.Expressions;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class PutItemRequest
    {
        public PutItemRequest(string tableName, AttributeCollection item)
        {
            #region Preconditions

            if (tableName == null) throw new ArgumentNullException("tableName");
            if (item == null) throw new ArgumentNullException("item");

            #endregion

            TableName = tableName;
            Item = item;
        }

        public string TableName { get; }

        public AttributeCollection Item { get; }

        public string ConditionExpression { get; set; }

        public JsonObject ExpressionAttributeNames { get; set; }

        public AttributeCollection ExpressionAttributeValues { get; set; }

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
                json.Add("ReturnValues", ReturnValues.ToString());
            }

            return json;
        }
    }
}
