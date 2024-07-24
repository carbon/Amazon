using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public readonly struct Tag
{
    [JsonPropertyName("Key")]
    [StringLength(128)]
    public required string Key { get; init; }

    [JsonPropertyName("Value")]
    [StringLength(256)]
    public required string Value { get; init; }
}