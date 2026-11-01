namespace Amazon.DynamoDb;

public sealed class PutItemResult : IConsumedResources
{
    public AttributeCollection? Attributes { get; init; }

    public ConsumedCapacity? ConsumedCapacity { get; init; }
}
