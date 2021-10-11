using System.Text.Json.Serialization;

namespace Amazon.Ssm;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PingStatus
{
    Online,
    ConnectionLost,
    Inactive
}