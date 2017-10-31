using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class UpdateItemResult : IConsumedResources
    {
        public UpdateItemResult(AttributeCollection attributes, ConsumedCapacity consumedCapacity)
        {
            Attributes = attributes;
            ConsumedCapacity = consumedCapacity;
        }

        public ConsumedCapacity ConsumedCapacity { get; }

        public AttributeCollection Attributes { get; }

        public static UpdateItemResult FromJson(JsonObject json)
        {
            ConsumedCapacity consumedCapacity = null;
            AttributeCollection attributes = null;

            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityNode))
            {
                consumedCapacity = ConsumedCapacity.FromJson((JsonObject)consumedCapacityNode);
            }

            if (json.TryGetValue("Attributes", out var attributesNode))
            {
                attributes = AttributeCollection.FromJson((JsonObject)attributesNode);
            }

            return new UpdateItemResult(attributes, consumedCapacity);
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
