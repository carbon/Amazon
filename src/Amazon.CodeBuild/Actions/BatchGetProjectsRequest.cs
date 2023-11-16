using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class BatchGetProjectsRequest(params string[] names) : ICodeBuildRequest
{
    [JsonPropertyName("names")]
    public string[] Names { get; } = names ?? throw new ArgumentNullException(nameof(names));
}