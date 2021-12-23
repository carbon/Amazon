using System.Text.Json.Serialization;

namespace Amazon.Ses;

public readonly struct SesVerdict
{
    [JsonConstructor]
    public SesVerdict(string status)
    {
        Status = status;
    }

    [JsonPropertyName("status")]
    public string Status { get; }
}