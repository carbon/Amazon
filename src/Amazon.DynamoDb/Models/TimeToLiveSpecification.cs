using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class TimeToLiveSpecification
    {
        public TimeToLiveSpecification(string attributeName, bool enabled)
        {
            AttributeName = attributeName;
            Enabled = enabled;
        }

        public string AttributeName { get; }
        public bool Enabled { get; }

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
    }
}
