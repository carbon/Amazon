using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

[method: JsonConstructor]
public sealed class PutRequest(AttributeCollection item) : ItemRequest
{
    public AttributeCollection Item { get; } = item;
}