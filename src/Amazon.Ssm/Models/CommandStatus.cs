using System.Text.Json.Serialization;

namespace Amazon.Ssm;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommandStatus
{
    Pending,
    InProgress,
    Success,
    Cancelled,
    Failed,
    TimedOut,
    Cancelling
}