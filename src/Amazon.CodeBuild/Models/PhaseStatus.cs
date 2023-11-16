#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter<PhaseStatus>))]
public enum PhaseStatus
{
    SUCCEEDED = 1,
    FAILED = 2,
    FAULT = 3,
    TIMED_OUT = 4,
    IN_PROGRESS = 5,
    STOPPED = 6
}