using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class CreateProjectRequest : ICodeBuildRequest
{
    [JsonPropertyName("artifacts")]
    public required ProjectArtifacts Artifacts { get; set; }

    [JsonPropertyName("environment")]
    public required ProjectEnvironment Environment { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    [JsonPropertyName("encryptionKey")]
    public string? EncryptionKey { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("serviceRole")]
    public required string ServiceRole { get; set; }

    [JsonPropertyName("source")]
    public required ProjectSource Source { get; set; }

    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Tag[]? Tags { get; set; }

    [JsonPropertyName("badgeEnabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BadgeEnabled { get; set; }

    [JsonPropertyName("timeoutInMinutes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Range(5, 480)]
    public int? TimeoutInMinutes { get; set; }
}