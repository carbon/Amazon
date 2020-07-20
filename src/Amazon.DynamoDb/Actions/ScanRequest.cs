using System;
using System.Collections.Generic;
using Carbon.Data.Expressions;
using Carbon.Json;

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

            if (conditions != null)
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

        public int Limit { get; set; }

        public int? Segment { get; set; }

        public int? TotalSegments { get; set; }

        public AttributeCollection? ExclusiveStartKey { get; set; }

        public string? FilterExpression { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public AttributeCollection? ExpressionAttributeValues { get; set; }

        public SelectEnum Select { get; set; }

        public string? ProjectionExpression { get; set; }
    }
}
