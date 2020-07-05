#nullable disable

using Amazon.DynamoDb.Extensions;
using Amazon.DynamoDb.Models;
using Carbon.Json;
using System;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class DescribeTimeToLiveResult : IConvertibleFromJson
    {
        public DescribeTimeToLiveResult() { }

        public TimeToLiveDescription TimeToLiveDescription { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("TimeToLiveDescription"))
                TimeToLiveDescription = property.Value.GetObject<TimeToLiveDescription>();
        }
    }
}
