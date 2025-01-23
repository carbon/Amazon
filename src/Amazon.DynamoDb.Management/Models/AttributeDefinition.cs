namespace Amazon.DynamoDb.Models;

public sealed class AttributeDefinition
{
    public required string AttributeName { get; init; }

    public required AttributeType AttributeType { get; init; }
}