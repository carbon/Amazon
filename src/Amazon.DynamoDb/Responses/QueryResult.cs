#nullable disable
using System;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class QueryResult
    {
        public QueryResult() { }
        public QueryResult(
            ConsumedCapacity? consumedCapacity,
            AttributeCollection[] items,
            AttributeCollection? lastEvaluatedKey,
            int count)
        {
            ConsumedCapacity = consumedCapacity;
            LastEvaluatedKey = lastEvaluatedKey;
            Items = items;
            Count = count;
        }

        public ConsumedCapacity? ConsumedCapacity { get; }

        public AttributeCollection[] Items { get; }

        public AttributeCollection? LastEvaluatedKey { get; }

        public int Count { get; }

        public static QueryResult FromJsonElement(JsonElement json)
        {
            ConsumedCapacity? consumedCapacity = null;
            AttributeCollection? lastEvaluatedKey = null;

            if (json.TryGetProperty("ConsumedCapacity", out JsonElement consumedCapacityEl))
            {
                consumedCapacity = ConsumedCapacity.FromJsonElement(consumedCapacityEl);
            }

            if (json.TryGetProperty("LastEvaluatedKey", out JsonElement lastEvaluatedKeyEl))
            {
                lastEvaluatedKey = AttributeCollection.FromJsonElement(lastEvaluatedKeyEl);
            }

            AttributeCollection[] items;

            if (json.TryGetProperty("Items", out JsonElement itemsEl))
            {
                items = new AttributeCollection[itemsEl.GetArrayLength()];

                int i = 0;

                foreach (var itemEl in itemsEl.EnumerateArray())
                {
                    items[i] = AttributeCollection.FromJsonElement(itemEl);

                    i++;
                }
            }
            else
            {
                items = Array.Empty<AttributeCollection>();
            }

            return new QueryResult(
                consumedCapacity : consumedCapacity,
                items            : items,
                lastEvaluatedKey : lastEvaluatedKey,
                count            : json.GetProperty("Count").GetInt32()
            );
        }
    }
}

/* 
{ 
	"Count": 343,
	"ConsumedCapacity": {
		"TableName": "Thread",
		"CapacityUnits": 1
	},
	"Items": [
		{ 
			"hitCount": {"N":"225"},
			"date": {"S":"2011-05-31T00:00:00Z"},
			"siteId": {"N":"221051"}
		},
		{
			"hitCount": {"N":"120"},
			"date": {"S":"2011-06-01T00:00:00Z"},
			"siteId": {"N":"221051"}
		},
		{
			"hitCount": {"N":"6680"},
			"date": {"S":"2011-06-02T00:00:00Z"},
			"siteId": {"N":"221051"}
		}
	],
	"LastEvaluatedKey": {
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
	}
}
*/
