#nullable disable
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class UpdateItemResult : IConsumedResources
    {
        public UpdateItemResult() { }
        public UpdateItemResult(AttributeCollection attributes, ConsumedCapacity consumedCapacity)
        {
            Attributes = attributes;
            ConsumedCapacity = consumedCapacity;
        }

        public AttributeCollection Attributes { get; set; }

        public ConsumedCapacity ConsumedCapacity { get; set; }
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
