using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class StopBuildRequest : ICodeBuildRequest
{
    public StopBuildRequest(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        Id = id;
    }

    [JsonPropertyName("id")]
    public string Id { get; }
}