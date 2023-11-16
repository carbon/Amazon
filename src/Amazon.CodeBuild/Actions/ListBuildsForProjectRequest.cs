using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ListBuildsForProjectRequest : ICodeBuildRequest
{
    public ListBuildsForProjectRequest(
        string projectName,
        SortOrder? sortOrder = null,
        string? nextToken = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectName);

        ProjectName = projectName;
        SortOrder = sortOrder;
        NextToken = nextToken;
    }

    [JsonPropertyName("projectName")]
    public string ProjectName { get; }

    [JsonPropertyName("sortOrder")]
    public SortOrder? SortOrder { get; }

    [JsonPropertyName("nextToken")]
    public string? NextToken { get; }
}