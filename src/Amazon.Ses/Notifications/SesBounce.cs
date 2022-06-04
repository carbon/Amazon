#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SesBounce
{
    [JsonPropertyName("bounceType")]
    public SesBounceType BounceType { get; set; }

    [JsonPropertyName("bounceSubType")]
    public SesBounceSubtype BounceSubType { get; set; }

    [JsonPropertyName("bouncedRecipients")]
    public BouncedRecipient[] BouncedRecipients { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("reportingMTA")]
    public string ReportingMta { get; set; }
}