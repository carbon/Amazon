using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ProjectEnvironment
{
    [JsonPropertyName("computeType")]
    public required string ComputeType { get; init; }

    [JsonPropertyName("image")]
    public required string Image { get; init; }

    [JsonPropertyName("certificate")]
    public string? Certificate { get; init; }

    [JsonPropertyName("type")]
    public required ProjectEnvironmentType Type { get; init; }

    [JsonPropertyName("environmentVariables")]
    public EnvironmentVariable[]? EnvironmentVariables { get; init; }
}