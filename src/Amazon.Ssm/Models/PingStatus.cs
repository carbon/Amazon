using System.Text.Json.Serialization;

namespace Amazon.Ssm;

[JsonConverter(typeof(JsonStringEnumConverter<PingStatus>))]
public enum PingStatus
{
    Online,
    ConnectionLost,
    Inactive
}