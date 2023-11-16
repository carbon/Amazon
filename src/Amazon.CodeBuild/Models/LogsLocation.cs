using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class LogsLocation
{
    [JsonPropertyName("deepLink")]
    public string? DeepLink { get; init; }

    [JsonPropertyName("groupName")]
    public string? GroupName { get; init; }

    /// <summary>
    /// The name of the CloudWatch Logs stream for the build logs.
    /// </summary>
    [JsonPropertyName("streamName")]
    public string? StreamName { get; init; }
}