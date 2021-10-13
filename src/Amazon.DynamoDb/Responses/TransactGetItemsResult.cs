namespace Amazon.DynamoDb;

public sealed class TransactGetItemsResult : IConsumedResources
{
    public ConsumedCapacity? ConsumedCapacity { get; init; }

    public ItemResult[]? Responses { get; init; }
}

public sealed class ItemResult
{
    public AttributeCollection? Item { get; init; }
}
