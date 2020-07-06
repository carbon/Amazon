#nullable disable

using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class AttributeDefinition : IConvertibleFromJson
    {
        public string AttributeName { get; set; }
        public AttributeType AttributeType { get; set; }

        public JsonObject ToJson()
        {
            return new JsonObject()
            {
                { "AttributeName", AttributeName },
                { "AttributeType", AttributeType.ToQuickString() }
            };
        }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("AttributeName"))
            {
                AttributeName = property.Value.ToString();
            }
            else if (property.NameEquals("AttributeType"))
            {
                AttributeType = property.Value.GetEnum<AttributeType>();
            }
        }
    }
}
