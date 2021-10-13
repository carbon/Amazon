namespace Amazon.DynamoDb;

public sealed class PutRequest : ItemRequest
{
    public PutRequest(AttributeCollection item)
    {
        Item = item;
    }

    public AttributeCollection Item { get; }
}
