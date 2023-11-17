namespace Amazon.DynamoDb;

public sealed class TransactGetItemsResult : IConsumedResources
{
    public ConsumedCapacity? ConsumedCapacity { get; init; }

    public ItemResponse[]? Responses { get; init; }
}

public sealed class ItemResponse
{
    public AttributeCollection? Item { get; init; }
}