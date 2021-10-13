namespace Amazon.DynamoDb;

public sealed class DeleteRequest : ItemRequest
{
    public DeleteRequest(AttributeCollection key)
    {
        Key = key;
    }

    public AttributeCollection Key { get; }
}
