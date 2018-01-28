using System.Text;

using Carbon.Data.Expressions;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class DynamoQuery
    {
        public DynamoQuery() { }

        public DynamoQuery(params Expression[] keyConditions)
        {
            if (keyConditions.Length == 0) return;

            var x = DynamoExpression.Conjunction(keyConditions);

            if (x.HasAttributeNames) ExpressionAttributeNames = x.AttributeNames;

            if (x.HasAttributeValues) ExpressionAttributeValues = x.AttributeValues;

            KeyConditionExpression = x.Text;
        }

        // [Required]
        public string TableName { get; set; }

        public bool ConsistentRead { get; set; }

        public string IndexName { get; set; }

        public int Limit { get; set; }

        public string KeyConditionExpression { get; set; }

        public JsonObject ExpressionAttributeNames { get; set; }

        public AttributeCollection ExpressionAttributeValues { get; set; }

        public string FilterExpression { get; set; }

        /// <summary>
        /// Default is true (ascending).	
        /// </summary>
        public bool ScanIndexForward { get; set; } = true;

        public string ProjectionExpression { get; set; }

        public SelectEnum Select { get; set; }

        /// <summary>
        /// If set to TOTAL, ConsumedCapacity is included in the response; 
        /// if set to NONE (the default), ConsumedCapacity is not included.
        /// </summary>
        public bool ReturnConsumedCapacity { get; set; }

        public AttributeCollection ExclusiveStartKey { get; set; }

        #region Helpers

        public DynamoQuery Descending()
        {
            ScanIndexForward = false;

            return this;
        }

        #endregion

        #region Builder

        private DynamoExpression _filter;

        public DynamoQuery Filter(params Expression[] conditions)
        {
            if (_filter == null)
            {
                if (ExpressionAttributeNames == null)  ExpressionAttributeNames = new JsonObject();
                if (ExpressionAttributeValues == null) ExpressionAttributeValues = new AttributeCollection();

                _filter = new DynamoExpression(ExpressionAttributeNames, ExpressionAttributeValues);
            }

            foreach (var condition in conditions)
            {
                _filter.Add(condition);
            }

            this.FilterExpression = _filter.Text;

            return this;
        }

        public DynamoQuery Take(int take)
        {
            this.Limit = take;

            return this;
        }

        public DynamoQuery Include(params string[] values)
        {
            var sb = new StringBuilder();

            if (ExpressionAttributeNames == null)
            {
                ExpressionAttributeNames = new JsonObject();
            }

            var i = 0;

            foreach (var value in values)
            {
                if (i != 0) sb.Append(',');

                sb.WriteName(value, ExpressionAttributeNames);

                i++;
            }

            this.ProjectionExpression = sb.ToString();

            return this;
        }

        public DynamoQuery WithIndex(string name)
        {
            this.IndexName = name;

            return this;
        }

        #endregion

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "TableName", TableName }
            };

            if (KeyConditionExpression != null)
            {
                json.Add("KeyConditionExpression", KeyConditionExpression);
            }

            if (ExpressionAttributeNames != null && ExpressionAttributeNames.Keys.Count > 0)
            {
                json.Add("ExpressionAttributeNames", ExpressionAttributeNames);
            }

            if (ExpressionAttributeValues != null && ExpressionAttributeValues.Count > 0)
            {
                json.Add("ExpressionAttributeValues", ExpressionAttributeValues.ToJson());
            }

            if (FilterExpression != null) json.Add("FilterExpression", FilterExpression);

            if (Select != SelectEnum.Unknown) json.Add("Select", Select.ToString());

            if (ProjectionExpression != null) json.Add("ProjectionExpression", ProjectionExpression);

            if (Limit != 0)                 json.Add("Limit", Limit);
            if (IndexName != null)          json.Add("IndexName", IndexName);
            if (ConsistentRead)             json.Add("ConsistentRead", ConsistentRead);
            if (ExclusiveStartKey != null)  json.Add("ExclusiveStartKey", ExclusiveStartKey.ToJson());
            if (!ScanIndexForward)          json.Add("ScanIndexForward", ScanIndexForward);                      // Default = true
            if (ReturnConsumedCapacity)     json.Add("ReturnConsumedCapacity", "TOTAL");

            return json;
        }
    }

    public enum SelectEnum
    {
        Unknown = 0,

        /// <summary>
        /// Returns all of the item attributes. For a table, this is the default. 
        /// For an index, this mode causes Amazon DynamoDB to fetch the full item from the table for each matching item in the index.
        /// If the index is configured to project all item attributes, the matching items will not be fetched from the table.
        /// Fetching items from the table incurs additional throughput cost and latency.
        /// </summary>
        ALL_ATTRIBUTES = 1,

        /// <summary>
        /// Allowed only when querying an index. Retrieves all attributes which have been projected into the index. 
        /// If the index is configured to project all attributes, this is equivalent to specifying ALL_ATTRIBUTES.
        /// </summary>
        ALL_PROJECTED_ATTRIBUTES = 2,

        /// <summary>
        /// Returns the number of matching items, rather than the matching items themselves.
        /// </summary>
        COUNT = 3,

        /// <summary>
        /// Returns only the attributes listed in AttributesToGet.
        /// This is equivalent to specifying AttributesToGet without specifying any value for Select.
        /// </summary>
        SPECIFIC_ATTRIBUTES = 4
    }
}


// For a query on a table, you can only have conditions on the table primary key attributes. 
// you must specify the hash key attribute name and value as an EQ condition. 
// You can optionally specify a second condition, referring to the range key attribute.

/*
{
    "AttributesToGet": [ "string" ],
    "ConditionalOperator": "string",
    "ConsistentRead": "boolean",
    "ExclusiveStartKey": {
        "string" :
            {
                "B": "blob",
                "BS": [
                    "blob"
                ],
                "N": "string",
                "NS": [
                    "string"
                ],
                "S": "string",
                "SS": [
                    "string"
                ]
            }
    },
    "IndexName": "string",
  
    "Limit": "number",
    
    "ReturnConsumedCapacity": "string",
    "ScanIndexForward": "boolean",
    "Select": "string",
    "TableName": "string"
}
*/
