using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class GetItemResult : IConsumedResources
    {
        public ConsumedCapacity ConsumedCapacity { get; set; }

        public AttributeCollection Item { get; set; }

        public static GetItemResult FromJson(JsonObject json)
        {
            var result = new GetItemResult();

            if (json.ContainsKey("ConsumedCapacity"))
            {
                result.ConsumedCapacity = ConsumedCapacity.FromJson((JsonObject)json["ConsumedCapacity"]);
            }

            if (json.ContainsKey("Item"))
            {
                result.Item = AttributeCollection.FromJson((JsonObject)json["Item"]);
            }

            return result;
        }
    }
}

/*
{
    "ConsumedCapacity": {
        "CapacityUnits": "number",
        "TableName": "string"
    },
    "Item": {
        "string": {
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
