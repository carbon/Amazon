namespace Amazon.DynamoDb
{
    public sealed class TransactGetItemsResult : IConsumedResources
    {
        public ConsumedCapacity? ConsumedCapacity { get; set; }

        public ItemResult[]? Responses { get; set; }
    }

    public sealed class ItemResult
    {
        public AttributeCollection? Item { get; set; }
    }
}
