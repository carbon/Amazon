namespace Amazon.DynamoDb
{
    public sealed class GetItemResult : IConsumedResources
    {
#nullable disable

        public AttributeCollection Item { get; set; }

#nullable enable

        public ConsumedCapacity? ConsumedCapacity { get; set; }
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
