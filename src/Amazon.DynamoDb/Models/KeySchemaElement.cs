using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class KeySchemaElement
    {
        public KeySchemaElement(string attributeName, KeyType keyType)
        {
            AttributeName = attributeName ?? throw new ArgumentNullException(nameof(attributeName));
            KeyType = keyType;
        }

        public string AttributeName { get; set; }

        public KeyType KeyType { get; set; }

        public JsonObject ToJson()
        {
            return new JsonObject {
                { "AttributeName", AttributeName },
                { "KeyType", KeyType.ToQuickString() }
            };
        }
    }
}
