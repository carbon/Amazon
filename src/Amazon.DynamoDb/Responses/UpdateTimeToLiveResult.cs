#nullable disable

using Amazon.DynamoDb.Extensions;
using Amazon.DynamoDb.Models;
using Carbon.Json;
using System;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class UpdateTimeToLiveResult : IConvertibleFromJson
    {
        public TimeToLiveSpecification TimeToLiveSpecification { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("TimeToLiveSpecification"))
                TimeToLiveSpecification = property.Value.GetObject<TimeToLiveSpecification>();
        }
    }
}
