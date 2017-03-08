using Carbon.Json;

namespace Amazon.DynamoDb
{
    using System;
    using System.Collections.Generic;

    // Shared with ScanResult

    public class QueryResult : IConsumedResources
    {
        public QueryResult(
            ConsumedCapacity consumedCapacity,
            AttributeCollection[] items,
            AttributeCollection lastEvaluatedKey,
            int count)
        {
            ConsumedCapacity = consumedCapacity;
            LastEvaluatedKey = lastEvaluatedKey;
            Items = items;
            Count = count;
        }

        public ConsumedCapacity ConsumedCapacity { get; }

        public int Count { get; }

        public AttributeCollection[] Items { get; }

        public AttributeCollection LastEvaluatedKey { get; }

        public static QueryResult FromJson(JsonObject json)
        {
            ConsumedCapacity consumedCapacity = null;
            AttributeCollection lastEvaluatedKey = null;

            if (json.ContainsKey("ConsumedCapacity"))
            {
                consumedCapacity = ConsumedCapacity.FromJson((JsonObject)json["ConsumedCapacity"]);
            }

            if (json.ContainsKey("LastEvaluatedKey"))
            {
                lastEvaluatedKey = AttributeCollection.FromJson((JsonObject)json["LastEvaluatedKey"]);
            }

            AttributeCollection[] items;

            if (json.ContainsKey("Items"))
            {
                var itemsJson = (JsonArray)json["Items"];

                items = new AttributeCollection[itemsJson.Count];

                for (var i = 0; i < items.Length; i++)
                {
                    items[i] = AttributeCollection.FromJson((JsonObject)itemsJson[i]);
                }
            }
            else
            {
                items = Array.Empty<AttributeCollection>();
            }

            return new QueryResult(
                consumedCapacity: consumedCapacity,
                items: items,
                lastEvaluatedKey: lastEvaluatedKey,
                count: (int)json["Count"]
            );
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

    }
}
