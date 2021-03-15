using System;
using System.Collections.Generic;

using Carbon.Data.Expressions;

namespace Amazon.DynamoDb
{
    public sealed class ScanRequest
    {
        public ScanRequest(string tableName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public ScanRequest(string tableName, Expression[] conditions)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));

            if (conditions is not null)
            {
                SetFilterExpression(DynamoExpression.Conjunction(conditions));
            }
        }

        internal void SetFilterExpression(DynamoExpression expression)
        {
            FilterExpression = expression.Text;

            if (expression.HasAttributeNames)
            {
                ExpressionAttributeNames = expression.AttributeNames;
            }

            if (expression.HasAttributeValues)
            {
                ExpressionAttributeValues = expression.AttributeValues;
            }
        }

        public string TableName { get; }

        // Each scan may return upto 1MB of data

        public int? Limit { get; set; }

        public int? Segment { get; set; }

        public int? TotalSegments { get; set; }

        public IReadOnlyDictionary<string, DbValue>? ExclusiveStartKey { get; set; }

        public string? FilterExpression { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public AttributeCollection? ExpressionAttributeValues { get; set; }

        public SelectEnum? Select { get; set; }

        public string? ProjectionExpression { get; set; }
    }
}