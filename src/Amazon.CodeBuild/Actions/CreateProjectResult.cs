using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class CreateProjectResult
{
    [JsonPropertyName("project")]
    public required Project Project { get; init; }
}