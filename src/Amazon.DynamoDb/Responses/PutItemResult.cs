using Carbon.Json;

namespace Amazon.DynamoDb
{
    public sealed class PutItemResult : IConsumedResources
    {
        public PutItemResult(AttributeCollection attributes, ConsumedCapacity? consumedCapacity)
        {
            Attributes = attributes;
            ConsumedCapacity = consumedCapacity;
        }
        
        public AttributeCollection Attributes { get; }

        public ConsumedCapacity? ConsumedCapacity { get; }

        public static PutItemResult FromJson(JsonObject json)
        {
            AttributeCollection? attributes = null;
            ConsumedCapacity? consumedCapacity = null;

            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityNode))
            {
                consumedCapacity = consumedCapacityNode.As<ConsumedCapacity>();
            }

            if (json.TryGetValue("Attributes", out var attributeNode))
            {
                attributes = AttributeCollection.FromJson((JsonObject)attributeNode);
            }

            return new PutItemResult(attributes!, consumedCapacity);
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
