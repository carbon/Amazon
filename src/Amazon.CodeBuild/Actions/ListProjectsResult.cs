using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ListProjectsResult
{
    [JsonPropertyName("nextToken")]
    public string? NextToken { get; init; }

    [JsonPropertyName("projects")]
    public required string[] Projects { get; init; }
}