using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb.Models
{
    public class ProvisionedThroughputOverride : IConvertibleFromJson
    {
        public int ReadCapacityUnits { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("ReadCapacityUnits")) ReadCapacityUnits = property.Value.GetInt32();
        }

        public JsonObject ToJson()
        {
            return new JsonObject()
            {
                { "ReadCapacityUnits", ReadCapacityUnits }
            };
        }
    }
}
