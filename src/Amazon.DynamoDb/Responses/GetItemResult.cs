namespace Amazon.DynamoDb;

public sealed class GetItemResult : IConsumedResources
{
    public AttributeCollection? Item { get; init; }

    public ConsumedCapacity? ConsumedCapacity { get; init; }
}

// NOTE: If there is no matching item, GetItem does not return any data and there will be no Item element in the response.