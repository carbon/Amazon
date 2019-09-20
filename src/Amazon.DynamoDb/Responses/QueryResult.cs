using System;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public sealed class QueryResult
    {
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

        public static QueryResult FromJson(JsonObject json)
        {
            ConsumedCapacity? consumedCapacity = null;
            AttributeCollection? lastEvaluatedKey = null;

            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityNode))
            {
                consumedCapacity = consumedCapacityNode.As<ConsumedCapacity>();
            }

            if (json.TryGetValue("LastEvaluatedKey", out var lastEvaluatedKeyNode))
            {
                lastEvaluatedKey = AttributeCollection.FromJson((JsonObject)lastEvaluatedKeyNode);
            }

            AttributeCollection[] items;

            if (json.TryGetValue("Items", out var itemsNode))
            {
                var itemsJson = (JsonArray)itemsNode;

                items = new AttributeCollection[itemsJson.Count];

                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = AttributeCollection.FromJson((JsonObject)itemsJson[i]);
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
                count            : (int)json["Count"]
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
