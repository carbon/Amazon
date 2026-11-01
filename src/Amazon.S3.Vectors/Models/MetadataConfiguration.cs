using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors.Models;

public class MetadataConfiguration
{
    [JsonPropertyName("nonFilterableMetadataKeys")]
    public required List<string> NonFilterableMetadataKeys { get; init; }
}
