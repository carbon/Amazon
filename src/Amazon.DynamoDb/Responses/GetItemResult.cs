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
            
            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityNode))
            {
                result.ConsumedCapacity = consumedCapacityNode.As<ConsumedCapacity>();
            }

            if (json.TryGetValue("Item", out var itemNode))
            {
                result.Item = AttributeCollection.FromJson((JsonObject)itemNode);
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
