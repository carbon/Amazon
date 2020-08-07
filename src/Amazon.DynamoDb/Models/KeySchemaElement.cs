using System;

namespace Amazon.DynamoDb.Models
{
    public sealed class KeySchemaElement
    {
        public KeySchemaElement() { }

        public KeySchemaElement(string attributeName, KeyType keyType)
        {
            AttributeName = attributeName ?? throw new ArgumentNullException(nameof(attributeName));
            KeyType = keyType;
        }

        public string? AttributeName { get; set; }

        public KeyType KeyType { get; set; }
    }
}