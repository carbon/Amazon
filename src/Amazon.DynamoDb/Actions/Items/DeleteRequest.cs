using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

[method: JsonConstructor]
public sealed class DeleteRequest(AttributeCollection key) : ItemRequest
{
    public AttributeCollection Key { get; } = key;
}