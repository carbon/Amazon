using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ListBuildsRequest : ICodeBuildRequest
{
    [JsonPropertyName("nextToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? NextToken { get; set; }

    [JsonPropertyName("sortOrder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SortOrder? SortOrder { get; set; }
}