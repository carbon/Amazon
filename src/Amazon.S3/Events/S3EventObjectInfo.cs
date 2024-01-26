#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.S3.Events;

public sealed class S3EventObjectInfo
{
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("size")]
    public long Size { get; init; }

    [JsonPropertyName("eTag")]
    public string ETag { get; init; }

    [JsonPropertyName("versionId")]
    public string VersionId { get; init; }

    [JsonPropertyName("sequencer")]
    public string Sequencer { get; init; }
}