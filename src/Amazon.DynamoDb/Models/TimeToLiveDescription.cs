using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class TimeToLiveDescription : IConvertibleFromJson
    {
        public string? AttributeName { get; set; }
        public TimeToLiveStatus TimeToLiveStatus { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("AttributeName")) AttributeName = property.Value.GetString();
            else if (property.NameEquals("TimeToLiveStatus")) TimeToLiveStatus = property.Value.GetEnum<TimeToLiveStatus>();
        }
    }
}
