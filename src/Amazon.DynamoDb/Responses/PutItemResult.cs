namespace Amazon.DynamoDb
{
    public sealed class PutItemResult : IConsumedResources
    {
        public AttributeCollection? Attributes { get; }

        public ConsumedCapacity? ConsumedCapacity { get; }
    }
}