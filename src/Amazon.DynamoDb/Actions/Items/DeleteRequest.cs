using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

public sealed class DeleteRequest : ItemRequest
{
    [JsonConstructor]
    public DeleteRequest(AttributeCollection key)
    {
        Key = key;
    }

    public AttributeCollection Key { get; }
}
