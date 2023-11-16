using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ListBuildsForProjectResult
{
    [JsonPropertyName("ids")]
    public required string[] Ids { get; init; }

    [JsonPropertyName("nextToken")]
    public string? NextToken { get; init; }
}