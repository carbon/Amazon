using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models;

[JsonConverter(typeof(JsonStringEnumConverter<TimeToLiveStatus>))]
public enum TimeToLiveStatus
{
    ENABLING = 1,
    DISABLING = 2,
    ENABLED = 3,
    DISABLED = 4
}