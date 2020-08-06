using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReturnConsumedCapacity
    {
        INDEXES,
        TOTAL,
        NONE
    }
}
