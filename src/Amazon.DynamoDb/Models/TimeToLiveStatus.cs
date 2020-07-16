using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TimeToLiveStatus : byte
    {
        ENABLING,
        DISABLING,
        ENABLED,
        DISABLED,
    };
}
