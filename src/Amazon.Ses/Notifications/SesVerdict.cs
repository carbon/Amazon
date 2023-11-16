using System.Text.Json.Serialization;

namespace Amazon.Ses;

[method: JsonConstructor]
public readonly struct SesVerdict(string status)
{
    [JsonPropertyName("status")]
    public string Status { get; } = status;
}