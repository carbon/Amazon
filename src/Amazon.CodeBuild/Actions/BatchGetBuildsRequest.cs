using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class BatchGetBuildsRequest(params string[] ids) : ICodeBuildRequest
{
    [JsonPropertyName("ids")]
    public string[] Ids { get; } = ids ?? throw new ArgumentNullException(nameof(ids));
}