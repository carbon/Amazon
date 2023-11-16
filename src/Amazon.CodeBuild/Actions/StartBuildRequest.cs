using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class StartBuildRequest : ICodeBuildRequest
{
    public StartBuildRequest() { }

    [SetsRequiredMembers]
    public StartBuildRequest(string projectName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(projectName);

        ProjectName = projectName;
    }

    [JsonPropertyName("projectName")]
    public required string ProjectName { get; init; }

    [JsonPropertyName("artifactsOverride")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProjectArtifacts? ArtifactsOverride { get; set; }

    [JsonPropertyName("buildspecOverride")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BuildspecOverride { get; set; }

    [JsonPropertyName("environmentVariablesOverride")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EnvironmentVariable[]? EnvironmentVariablesOverride { get; set; }

    [JsonPropertyName("sourceVersion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceVersion { get; set; }

    [JsonPropertyName("timeoutInMinutesOverride")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TimeoutInMinutesOverride { get; set; }

    [JsonPropertyName("idempotencyToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IdempotencyToken { get; set; }
}