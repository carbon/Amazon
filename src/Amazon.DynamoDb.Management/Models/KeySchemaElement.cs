namespace Amazon.DynamoDb.Models;

public sealed class KeySchemaElement
{
    public KeySchemaElement() { }

    public KeySchemaElement(string attributeName, KeyType keyType)
    {
        ArgumentNullException.ThrowIfNull(attributeName);

        AttributeName = attributeName;
        KeyType = keyType;
    }

    public string? AttributeName { get; set; }

    public KeyType KeyType { get; set; }
}