using System;
using System.Collections.Generic;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Amazon.DynamoDb
{
    public sealed class UpdateItemRequest
    {
        public UpdateItemRequest(
            string tableName, 
            IReadOnlyDictionary<string, DbValue> key,
            Change[] changes,
            Expression[]? conditions = null,
            ReturnValues? returnValues = null)
        {
            TableName    = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key          = key       ?? throw new ArgumentNullException(nameof(key));
            ReturnValues = returnValues;

            var attrNames = new Dictionary<string, string>();

            var updateExpression = new UpdateExpression(changes, attrNames, ExpressionAttributeValues);

            UpdateExpression = updateExpression.ToString();

            if (conditions is { Length: > 0 })
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

        public IReadOnlyDictionary<string, DbValue> Key { get; }

        public string? ConditionExpression { get; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; }

        public AttributeCollection ExpressionAttributeValues { get; } = new AttributeCollection();

        public ReturnValues? ReturnValues { get; }

        public string UpdateExpression { get; }
    }
}