using System.Text.Json;

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

        public static GetItemResult FromJsonElement(JsonElement json)
        {
            AttributeCollection? item = null;
            ConsumedCapacity? consumedCapacity = null;

            if (json.TryGetProperty("ConsumedCapacity", out var consumedCapacityEl))
            {
                consumedCapacity = ConsumedCapacity.FromJsonElement(consumedCapacityEl);
            }

            if (json.TryGetProperty("Item", out var itemEl))
            {
                item = AttributeCollection.FromJsonElement(itemEl);
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
