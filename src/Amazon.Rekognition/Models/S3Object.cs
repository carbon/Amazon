using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[method: JsonConstructor]
public readonly struct S3Object(string bucket, string name, string? version = null)
{
    [JsonPropertyName("Bucket")]
    public string Bucket { get; } = bucket;

    [JsonPropertyName("Name")]
    public string Name { get; } = name;

    [JsonPropertyName("Version")]
    public string? Version { get; } = version;
}
