#nullable disable
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class UpdateItemResult : IConsumedResources
    {
        public UpdateItemResult() { }
        public UpdateItemResult(AttributeCollection? attributes, ConsumedCapacity? consumedCapacity)
        {
            Attributes = attributes;
            ConsumedCapacity = consumedCapacity;
        }

        public AttributeCollection? Attributes { get; }

        public ConsumedCapacity? ConsumedCapacity { get; }

        public static UpdateItemResult FromJsonElement(JsonElement json)
        {
            ConsumedCapacity? consumedCapacity = null;
            AttributeCollection? attributes = null;

            if (json.TryGetProperty("ConsumedCapacity", out var consumedCapacityEl))
            {
                consumedCapacity = ConsumedCapacity.FromJsonElement(consumedCapacityEl);
            }

            if (json.TryGetProperty("Attributes", out var attributesEl))
            {
                attributes = AttributeCollection.FromJsonElement(attributesEl);
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
