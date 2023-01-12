using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ProjectEnvironment
{
    public required string ComputeType { get; init; }

    public required string Image { get; init; }

    [JsonPropertyName("certificate")]
    public string? Certificate { get; init; }

    [JsonPropertyName("type")]
    public required ProjectEnvironmentType Type { get; init; }

    public EnvironmentVariable[]? EnvironmentVariables { get; init; }
}