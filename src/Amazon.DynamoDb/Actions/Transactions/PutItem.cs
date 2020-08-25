using System;
using System.Collections.Generic;

namespace Amazon.DynamoDb.Transactions
{
    public sealed class PutItem
    {
        public PutItem(string tableName, AttributeCollection item)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }

        public AttributeCollection Item { get; set; }

        public string TableName { get; set; }

        public string? ConditionExpression { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public AttributeCollection? ExpressionAttributeValues { get; set; }

        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }
    }
}