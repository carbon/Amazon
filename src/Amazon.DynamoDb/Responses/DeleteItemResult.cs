namespace Amazon.DynamoDb;

public sealed class DeleteItemResult : IConsumedResources
{
    public AttributeCollection? Attributes { get; init; }

    public ConsumedCapacity? ConsumedCapacity { get; init; }
}
