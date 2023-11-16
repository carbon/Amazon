#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public class Project
{
    [JsonPropertyName("arn")]
    public string Arn { get; init; }

    [JsonPropertyName("artifacts")]
    public ProjectArtifacts Artifacts { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

#nullable enable

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("encryptionKey")]
    public string? EncryptionKey { get; init; }

    [JsonPropertyName("environment")]
    public ProjectEnvironment? Environment { get; init; }

    [JsonPropertyName("created")]
    public Timestamp Created { get; init; }

    [JsonPropertyName("lastModified")]
    public Timestamp LastModified { get; init; }

    [JsonPropertyName("serviceRole")]
    public string? ServiceRole { get; init; }

    [JsonPropertyName("tags")]
    public Tag[]? Tags { get; init; }

    [JsonPropertyName("timeoutInMinutes")]
    public int TimeoutInMinutes { get; init; }

    [JsonPropertyName("webhook")]
    public Webhook? Webhook { get; set; }
}