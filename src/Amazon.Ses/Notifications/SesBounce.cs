#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SesBounce
{
    [JsonPropertyName("bounceType")]
    public SesBounceType BounceType { get; init; }

    [JsonPropertyName("bounceSubType")]
    public SesBounceSubtype BounceSubType { get; init; }

    [JsonPropertyName("bouncedRecipients")]
    public BouncedRecipient[] BouncedRecipients { get; init; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; }

    [JsonPropertyName("reportingMTA")]
    public string ReportingMta { get; init; }
}