using Carbon.Json;

namespace Amazon.DynamoDb
{
    public sealed class GetItemResult : IConsumedResources
    {
        public GetItemResult(AttributeCollection item, ConsumedCapacity? consumedCapacity)
        {
            Item = item;
            ConsumedCapacity = consumedCapacity;
        }

        public AttributeCollection Item { get; }

        public ConsumedCapacity? ConsumedCapacity { get; }

        public static GetItemResult FromJson(JsonObject json)
        {
            AttributeCollection? item = null;
            ConsumedCapacity? consumedCapacity = null;

            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityNode))
            {
                consumedCapacity = consumedCapacityNode.As<ConsumedCapacity>();
            }

            if (json.TryGetValue("Item", out var itemNode))
            {
                item = AttributeCollection.FromJson((JsonObject)itemNode);
            }

            return  new GetItemResult(item!, consumedCapacity);
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
