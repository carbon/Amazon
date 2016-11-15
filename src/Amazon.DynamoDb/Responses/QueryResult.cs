using Carbon.Json;

namespace Amazon.DynamoDb
{
    using System.Collections.Generic;

    // Shared with ScanResult

    public class QueryResult : IConsumedResources
    {
        public ConsumedCapacity ConsumedCapacity { get; set; }

        public AttributeCollection LastEvaluatedKey { get; set; }

        public int Count { get; set; }

        public IList<AttributeCollection> Items { get; } = new List<AttributeCollection>();

        public static QueryResult FromJson(JsonObject json)
        {
            var result = new QueryResult {
                Count = (int)json["Count"]
            };

            if (json.ContainsKey("ConsumedCapacity"))
            {
                result.ConsumedCapacity = ConsumedCapacity.FromJson((JsonObject)json["ConsumedCapacity"]);
            }

            if (json.ContainsKey("LastEvaluatedKey"))
            {
                result.LastEvaluatedKey = AttributeCollection.FromJson((JsonObject)json["LastEvaluatedKey"]);
            }

            if (json.ContainsKey("Items"))
            {
                foreach (var item in (JsonArray)json["Items"])
                {
                    result.Items.Add(AttributeCollection.FromJson((JsonObject)item));
                }
            }

            return result;
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
