namespace Amazon.DynamoDb;

public sealed class GetItemResult : IConsumedResources
{
#nullable disable
    public AttributeCollection Item { get; init; }
#nullable enable

    public ConsumedCapacity? ConsumedCapacity { get; init; }
}
