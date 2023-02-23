using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public readonly struct S3Object
{
    [JsonConstructor]
    public S3Object(string bucket, string name, string? version = null)
    {
        Bucket = bucket;
        Name = name;
        Version = version;
    }

    [JsonPropertyName("Bucket")]
    public string Bucket { get; }

    [JsonPropertyName("Name")]
    public string Name { get; }

    [JsonPropertyName("Version")]
    public string? Version { get; }
}
