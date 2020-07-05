#nullable disable

using Carbon.Json;
using System.Text.Json;

namespace Amazon.DynamoDb.Models
{
    public class TimeToLiveSpecification : IConvertibleFromJson
    {
        public TimeToLiveSpecification() { }
        public TimeToLiveSpecification(string attributeName, bool enabled)
        {
            AttributeName = attributeName;
            Enabled = enabled;
        }

        public string AttributeName { get; set; }
        public bool Enabled { get; set; }

        public JsonObject ToJson()
        {
            return new JsonObject
            {
                { "AttributeName", AttributeName },
                { "Enabled", Enabled },
            };
        }

        public static TimeToLiveSpecification FromJson(JsonNode json)
        {
            string attributeName = "";
            if (json.TryGetValue("AttributeName", out var attributeNameValue))
            {
                attributeName = attributeNameValue.ToString();
            }

            bool enabled = false;
            if (json.TryGetValue("Enabled", out var enabledValue))
            {
                enabled = enabledValue.As<bool>();
            }

            return new TimeToLiveSpecification(attributeName, enabled);
        }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("AttributeName")) AttributeName = property.Value.GetString();
            else if (property.NameEquals("Enabled")) Enabled = property.Value.GetBoolean();
        }
    }
}
