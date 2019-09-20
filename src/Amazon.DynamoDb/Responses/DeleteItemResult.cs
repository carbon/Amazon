using Carbon.Json;

namespace Amazon.DynamoDb
{
    public sealed class DeleteItemResult : IConsumedResources
    {
        public DeleteItemResult(AttributeCollection attributes, ConsumedCapacity? consumedCapacity)
        {
            Attributes = attributes;
            ConsumedCapacity = consumedCapacity;
        }
        
        public AttributeCollection Attributes { get; }

        public ConsumedCapacity? ConsumedCapacity { get; }

        public static DeleteItemResult FromJson(JsonObject json)
        {
            AttributeCollection? attributes = null;
            ConsumedCapacity? consumedCapacity = null;

            if (json.TryGetValue("Attributes", out var attributesNode))
            {
                attributes = AttributeCollection.FromJson((JsonObject)attributesNode);
            }

            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityNode))
            {
                consumedCapacity = consumedCapacityNode.As<ConsumedCapacity>();
            }

            return new DeleteItemResult(attributes!, consumedCapacity);
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
