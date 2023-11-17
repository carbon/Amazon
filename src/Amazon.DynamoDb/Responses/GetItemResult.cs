namespace Amazon.DynamoDb;

public sealed class GetItemResult : IConsumedResources
{
    public required AttributeCollection Item { get; init; }

    public ConsumedCapacity? ConsumedCapacity { get; init; }
}