#nullable disable

using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class KeySchemaElement
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
