#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class BatchGetBuildsResponse
{
    [JsonPropertyName("builds")]
    public Build[] Builds { get; init; }
}