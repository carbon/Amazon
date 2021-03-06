﻿using System;
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

        public AttributeCollection Item { get; init; }

        public string TableName { get; init; }

        public string? ConditionExpression { get; init; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; init; }

        public AttributeCollection? ExpressionAttributeValues { get; init; }

        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; init; }
    }
}