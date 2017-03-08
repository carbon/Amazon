using System;
using Carbon.Data.Expressions;
using Carbon.Json;

namespace Amazon.DynamoDb
{

    public class ScanRequest
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

        public AttributeCollection ExclusiveStartKey { get; set; }

        public string FilterExpression { get; set; }

        public JsonObject ExpressionAttributeNames { get; set; }

        public AttributeCollection ExpressionAttributeValues { get; set; }

        public SelectEnum Select { get; set; }

        public string ProjectionExpression { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "TableName", TableName }
            };

            if (Limit != 0)                         json.Add("Limit", Limit);
            if (Select != SelectEnum.Unknown)       json.Add("Select", Select.ToString());
            if (Segment != null)                    json.Add("Segment", Segment.Value);
            if (TotalSegments != null)              json.Add("TotalSegments", TotalSegments.Value);
            if (ExclusiveStartKey != null)          json.Add("ExclusiveStartKey", ExclusiveStartKey.ToJson());

            if (ExpressionAttributeNames != null)   json.Add("ExpressionAttributeNames", ExpressionAttributeNames);
            if (ExpressionAttributeValues != null)  json.Add("ExpressionAttributeValues", ExpressionAttributeValues.ToJson());

            if (FilterExpression != null)           json.Add("FilterExpression", FilterExpression);

            if (ProjectionExpression != null)       json.Add("ProjectionExpression", ProjectionExpression);

            return json;
        }
    }
}
