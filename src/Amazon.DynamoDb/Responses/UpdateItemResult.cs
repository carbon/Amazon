#nullable disable

namespace Amazon.DynamoDb;

public sealed class UpdateItemResult : IConsumedResources
{
    public AttributeCollection Attributes { get; init; }

    public ConsumedCapacity? ConsumedCapacity { get; init; }
}