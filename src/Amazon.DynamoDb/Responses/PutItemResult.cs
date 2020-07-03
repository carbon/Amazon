using System.Text.Json;

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

        public static PutItemResult FromJsonElement(JsonElement json)
        {
            AttributeCollection? attributes = null;
            ConsumedCapacity? consumedCapacity = null;

            if (json.TryGetProperty("ConsumedCapacity", out var consumedCapacityEl))
            {
                consumedCapacity = ConsumedCapacity.FromJsonElement(consumedCapacityEl);
            }

            if (json.TryGetProperty("Attributes", out var attributeEl))
            {
                attributes = AttributeCollection.FromJsonElement(attributeEl);
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
