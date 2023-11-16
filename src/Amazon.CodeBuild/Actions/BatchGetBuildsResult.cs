using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class BatchGetBuildsResult
{
    [JsonPropertyName("builds")]
    public required Build[] Builds { get; init; }
}