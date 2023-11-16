#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class CreateProjectResponse
{
    [JsonPropertyName("project")]
    public Project Project { get; init; }
}