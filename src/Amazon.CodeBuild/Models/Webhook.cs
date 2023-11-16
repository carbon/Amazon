using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public class Webhook
{
    [JsonPropertyName("branchFilter")]
    public string? BranchFilter { get; set; }

    [JsonPropertyName("buildType")]
    public string? BuildType { get; set; }

    [JsonPropertyName("lastModifiedSecret")]
    public Timestamp LastModifiedSecret { get; init; }

    [JsonPropertyName("payloadUrl")]
    public string? PayloadUrl { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}