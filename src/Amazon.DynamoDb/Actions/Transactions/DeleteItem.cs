using System;
using System.Collections.Generic;

namespace Amazon.DynamoDb
{
    public sealed class DeleteItem
    {
        public DeleteItem(string tableName, AttributeCollection key)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public AttributeCollection Key { get; set; }

        public string TableName { get; set; }

        public string? ConditionExpression { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public AttributeCollection? ExpressionAttributeValues { get; set; }

        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }
    }
}