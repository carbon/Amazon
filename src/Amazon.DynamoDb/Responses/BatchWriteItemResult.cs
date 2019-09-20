#nullable disable

using System.Collections.Generic;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class BatchWriteItemResult // : IConsumedResources
    {
        public ConsumedCapacity[] ConsumedCapacity { get; set; }

        public List<TableRequests> UnprocessedItems { get; set; }

        public static BatchWriteItemResult FromJson(JsonObject json)
        {
            var unprocessed = new List<TableRequests>();

            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityNode)) // Array
            {
                foreach (var item in (JsonArray)consumedCapacityNode)
                {
                    var unit = item.As<ConsumedCapacity>();

                    // TODO
                }
            }

            if (json.TryGetValue("UnprocessedItems", out var unprocessedItemsNode))
            {
                foreach (var batch in (JsonObject)unprocessedItemsNode)
                {
                    unprocessed.Add(TableRequests.FromJson(batch.Key, (JsonArray)batch.Value));
                }
            }

            return new BatchWriteItemResult {
                UnprocessedItems = unprocessed
            };
        }
    }
}

/*
{
    "ConsumedCapacity": [
        {
            "CapacityUnits": "number",
            "TableName": "string"
        }
    ],
    "ItemCollectionMetrics": 
        {
            "string" :
                [
                    {
                        "ItemCollectionKey": 
                            {
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
                        "SizeEstimateRangeGB": [
                            "number"
                        ]
                    }
                ]
        },
    "UnprocessedItems": 
        {
            "string" :
                [
                    {
                        "DeleteRequest": {
                            "Key": 
                                {
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
                        },
                        "PutRequest": {
                            "Item": 
                                {
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
                    }
                ]
        }
}
*/



/*
{ 
 "UnprocessedItems":  { 
	"Slugs": [ 
		{ 
			"PutRequest": { "Item": { "name": { "S": "marcywilliams" }, "ownerId": { "N": "3033325" }, "type": { "N": "1" } } } 
		}, 
		{
			"PutRequest": { "Item": { "name": { "S": "arch9evans" }, "ownerId": { "N": "3033326" }, "type": { "N": "1" } } } 
		}
	] 
  } 
}
*/
