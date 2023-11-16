#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class Build
{
    [JsonPropertyName("arn")]
    public string Arn { get; init; }

    [JsonPropertyName("artifacts")]
    public BuildArtifacts Artifacts { get; init; }

    [JsonPropertyName("buildComplete")]
    public bool BuildComplete { get; init; }

    // SUCCEEDED | FAILED | FAULT | TIMED_OUT | IN_PROGRESS | STOPPED
    [JsonPropertyName("buildStatus")]
    public string BuildStatus { get; init; }

    [JsonPropertyName("cache")]
    public ProjectCache Cache { get; init; }

    [JsonPropertyName("currentPhase")]
    public string CurrentPhase { get; init; }

    [JsonPropertyName("environment")]
    public ProjectEnvironment Environment { get; init; }

    [JsonPropertyName("id")]
    public string Id { get; init; }

    /// <summary>
    /// The entity that started the build.
    /// </summary>
    [JsonPropertyName("initiator")]
    public string Initiator { get; init; }

    [JsonPropertyName("logs")]
    public LogsLocation Logs { get; init; }

    [JsonPropertyName("phases")]
    public BuildPhase[] Phases { get; init; }

    [JsonPropertyName("projectName")]
    public string ProjectName { get; init; }

    // Any version identifier for the version of the source code to be built.
    public string SourceVersion { get; init; }

    [JsonPropertyName("startTime")]
    public Timestamp StartTime { get; init; }

    [JsonPropertyName("endTime")]
    public Timestamp? EndTime { get; init; }

    [JsonPropertyName("timeoutInMinutes")]
    public int? TimeoutInMinutes { get; init; }
}