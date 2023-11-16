using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class BatchGetProjectsResult
{
    [JsonPropertyName("projects")]
    public required Project[] Projects { get; init; }
}