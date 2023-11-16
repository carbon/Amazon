using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class DeleteProjectRequest : ICodeBuildRequest
{
    public DeleteProjectRequest(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Name = name;
    }

    [JsonPropertyName("name")]
    public string Name { get; }
}