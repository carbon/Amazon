using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ListBuildsRequest : ICodeBuildRequest
{
    [JsonPropertyName("nextToken")]
    public string? NextToken { get; set; }

    [JsonPropertyName("sortOrder")]
    public SortOrder? SortOrder { get; set; }
}