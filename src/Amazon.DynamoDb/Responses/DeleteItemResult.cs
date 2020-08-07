namespace Amazon.DynamoDb
{
    public sealed class DeleteItemResult : IConsumedResources
    {
        public AttributeCollection? Attributes { get; set; }

        public ConsumedCapacity? ConsumedCapacity { get; set; }
    }
}