#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class BuildPhase
{
    public PhaseContext[] Contexts { get; init; }

    public long DurationInSeconds { get; init; }

    public Timestamp StartTime { get; init; }

    public Timestamp? EndTime { get; init; }

    public string PhaseStatus { get; init; }

    [JsonPropertyName("phaseType")]
    public string PhaseType { get; init; }
}