using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TimeToLiveStatus
    {
        ENABLING,
        DISABLING,
        ENABLED,
        DISABLED,
    };
}