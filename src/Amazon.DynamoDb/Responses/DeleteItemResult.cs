using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class DeleteItemResult : IConsumedResources
    {
        public ConsumedCapacity ConsumedCapacity { get; set; }

        public AttributeCollection Attributes { get; set; }

        public static DeleteItemResult FromJson(JsonObject json)
        {
            var result = new DeleteItemResult();

            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityNode))
            {
                result.ConsumedCapacity = ConsumedCapacity.FromJson((JsonObject)consumedCapacityNode);
            }

            if (json.TryGetValue("Attributes", out var attributesNode))
            {
                result.Attributes = AttributeCollection.FromJson((JsonObject)attributesNode);
            }

            return result;
        }
    }
}

/*
{
    "Attributes": 
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
    "ConsumedCapacity": {
        "CapacityUnits": "number",
        "TableName": "string"
    },
    "ItemCollectionMetrics": {
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
}
*/
