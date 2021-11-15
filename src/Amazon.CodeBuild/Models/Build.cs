#nullable disable

namespace Amazon.CodeBuild;

public sealed class Build
{
    public string Arn { get; init; }

    public BuildArtifacts Artifacts { get; init; }

    public bool BuildComplete { get; init; }

    // SUCCEEDED | FAILED | FAULT | TIMED_OUT | IN_PROGRESS | STOPPED
    public string BuildStatus { get; init; }

    public ProjectCache Cache { get; init; }

    public string CurrentPhase { get; init; }

    public ProjectEnvironment Environment { get; init; }

    public string Id { get; init; }

    /// <summary>
    /// The entity that started the build.
    /// </summary>
    public string Initiator { get; init; }

    public LogsLocation Logs { get; init; }

    public BuildPhase[] Phases { get; init; }

    public string ProjectName { get; init; }

    // Any version identifier for the version of the source code to be built.
    public string SourceVersion { get; init; }

    public Timestamp StartTime { get; init; }

    public Timestamp? EndTime { get; init; }

    public int? TimeoutInMinutes { get; init; }
}