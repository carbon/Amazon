using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

public sealed class PutRequest : ItemRequest
{
    [JsonConstructor]
    public PutRequest(AttributeCollection item)
    {
        Item = item;
    }

    public AttributeCollection Item { get; }
}
