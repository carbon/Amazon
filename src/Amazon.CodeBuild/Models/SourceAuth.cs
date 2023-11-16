using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class SourceAuth
{
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("resource")]
    public string? Resource { get; set; }
}