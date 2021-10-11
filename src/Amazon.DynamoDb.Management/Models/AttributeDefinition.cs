#nullable disable

namespace Amazon.DynamoDb.Models;

public sealed class AttributeDefinition
{
    public string AttributeName { get; init; }

    public AttributeType AttributeType { get; init; }
}
