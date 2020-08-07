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
