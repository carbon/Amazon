using System.Collections.Generic;

namespace Amazon.DynamoDb
{
    public sealed class QueryResult
    {
        public ConsumedCapacity? ConsumedCapacity { get; set; }

#nullable disable

        public AttributeCollection[] Items { get; set; }

        public Dictionary<string, DbValue> LastEvaluatedKey { get; set; }

        public int Count { get; set; }
    }
}