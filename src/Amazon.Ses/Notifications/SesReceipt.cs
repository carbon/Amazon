#nullable disable

using System;
using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SesReceipt
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; }

    [JsonPropertyName("processingTimeMillis")]
    public long ProcessingTimeMillis { get; init; }

    [JsonPropertyName("recipients")]
    public string[] Recipients { get; init; }

    [JsonPropertyName("spamVerdict")]
    public SesVerdict SpamVerdict { get; init; }

    [JsonPropertyName("virusVerdict")]
    public SesVerdict VirusVerdict { get; init; }

    [JsonPropertyName("spfVerdict")]
    public SesVerdict SpfVerdict { get; init; }

    [JsonPropertyName("dkimVerdict")]
    public SesVerdict DkimVerdict { get; init; }

    [JsonPropertyName("action")]
    public SesNotificationAction Action { get; init; }
}