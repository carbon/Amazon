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

            if (json.ContainsKey("ConsumedCapacity"))
            {
                result.ConsumedCapacity = ConsumedCapacity.FromJson((JsonObject)json["ConsumedCapacity"]);
            }

            if (json.ContainsKey("Attributes"))
            {
                result.Attributes = AttributeCollection.FromJson((JsonObject)json["Attributes"]);
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
