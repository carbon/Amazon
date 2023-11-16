using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class BuildPhase
{
    [JsonPropertyName("contexts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PhaseContext[]? Contexts { get; init; }

    [JsonPropertyName("durationInSeconds")]
    public int DurationInSeconds { get; init; }

    [JsonPropertyName("startTime")]
    public Timestamp StartTime { get; init; }

    [JsonPropertyName("endTime")]
    public Timestamp? EndTime { get; init; }

    [JsonPropertyName("phaseStatus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PhaseStatus PhaseStatus { get; init; }

    [JsonPropertyName("phaseType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PhaseType PhaseType { get; init; }
}