using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class CitationsConfig
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}