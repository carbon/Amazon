#nullable disable

using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Text.Json;

namespace Amazon.DynamoDb.Models
{
    public class KeySchemaElement : IConvertibleFromJson
    {
        public KeySchemaElement() { }
        public KeySchemaElement(string attributeName, KeyType keyType)
        {
            AttributeName = attributeName ?? throw new ArgumentNullException(nameof(attributeName));
            KeyType = keyType;
        }

        public string? AttributeName { get; set; }

        public KeyType KeyType { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("AttributeName")) AttributeName = property.Value.GetString();
            else if (property.NameEquals("KeyType")) KeyType = property.Value.GetEnum<KeyType>();
        }

        public JsonObject ToJson()
        {
            return new JsonObject {
                { "AttributeName", AttributeName },
                { "KeyType", KeyType.ToQuickString() }
            };
        }
    }
}
