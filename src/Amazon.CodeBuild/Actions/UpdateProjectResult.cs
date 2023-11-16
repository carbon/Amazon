using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class UpdateProjectResult
{
    [JsonPropertyName("project")]
    public required Project Project { get; init; }
}