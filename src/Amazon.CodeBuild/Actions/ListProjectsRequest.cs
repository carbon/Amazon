using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ListProjectsRequest : ICodeBuildRequest
{
    // NAME | CREATED_TIME | LAST_MODIFIED_TIME
    [JsonPropertyName("sortBy")]
    public string? SortBy { get; set; }

    [JsonPropertyName("sortOrder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SortOrder? SortOrder { get; set; }

    [JsonPropertyName("nextTime")]
    public string? NextToken { get; set; }
}