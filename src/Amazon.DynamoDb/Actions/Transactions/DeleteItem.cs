using System;
using System.Collections.Generic;

namespace Amazon.DynamoDb.Transactions
{
    public sealed class DeleteItem
    {
        public DeleteItem(string tableName, AttributeCollection key)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public AttributeCollection Key { get; init; }

        public string TableName { get; init; }

        public string? ConditionExpression { get; init; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; init; }

        public AttributeCollection? ExpressionAttributeValues { get; init; }

        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; init; }
    }
}