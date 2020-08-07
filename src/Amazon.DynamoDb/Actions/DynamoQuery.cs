using System.Collections.Generic;
using System.Text;

using Carbon.Data.Expressions;

namespace Amazon.DynamoDb
{
    public sealed class DynamoQuery
    {
#nullable disable

        public DynamoQuery() { }

        public DynamoQuery(params Expression[] keyConditions)
        {
            if (keyConditions.Length == 0) return;

            var x = DynamoExpression.Conjunction(keyConditions);

            if (x.HasAttributeNames) ExpressionAttributeNames = x.AttributeNames;

            if (x.HasAttributeValues) ExpressionAttributeValues = x.AttributeValues;

            KeyConditionExpression = x.Text;
        }

#nullable enable

        public string TableName { get; set; }

        public bool? ConsistentRead { get; set; }

        public string? IndexName { get; set; }

        public string KeyConditionExpression { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public AttributeCollection? ExpressionAttributeValues { get; set; }

        public string? FilterExpression { get; set; }

        /// <summary>
        /// Default is true (ascending).	
        /// </summary>
        public bool? ScanIndexForward { get; set; }

        public string ProjectionExpression { get; set; }

        public int? Limit { get; set; }

        public SelectEnum? Select { get; set; }

        /// <summary>
        /// If set to TOTAL, ConsumedCapacity is included in the response; 
        /// if set to NONE (the default), ConsumedCapacity is not included.
        /// </summary>
        public ReturnConsumedCapacity? ReturnConsumedCapacity { get; set; }

        public AttributeCollection? ExclusiveStartKey { get; set; }

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
            if (_filter is null)
            {
                Dictionary<string, string> attrNames = ExpressionAttributeNames ?? new Dictionary<string, string>();
                ExpressionAttributeValues ??= new AttributeCollection();

                _filter = new DynamoExpression(attrNames, ExpressionAttributeValues);
            }

            foreach (Expression condition in conditions)
            {
                _filter.Add(condition);

            }

            this.FilterExpression = _filter.Text;
            if (_filter.HasAttributeNames && ExpressionAttributeNames == null)
            {
                ExpressionAttributeNames = _filter.AttributeNames;
            }

            return this;
        }

        public DynamoQuery Take(int take)
        {
            this.Limit = take;

            return this;
        }

        public DynamoQuery Include(params string[] values)
        {
            ExpressionAttributeNames ??= new Dictionary<string, string>();

            var sb = StringBuilderCache.Aquire();

            int i = 0;

            foreach (string value in values)
            {
                if (i != 0) sb.Append(',');

                sb.WriteName(value, ExpressionAttributeNames);

                i++;
            }

            this.ProjectionExpression = StringBuilderCache.ExtractAndRelease(sb);

            return this;
        }

        public DynamoQuery WithIndex(string name)
        {
            this.IndexName = name;

            return this;
        }

        #endregion
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
