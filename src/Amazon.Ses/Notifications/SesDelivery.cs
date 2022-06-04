using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SesDelivery
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("processingTimeMillis")]
    public long ProcessingTimeMillis { get; set; }

    [JsonPropertyName("recipients")]
    public string[]? Recipients { get; set; }

    [JsonPropertyName("smtpResponse")]
    public string? SmtpResponse { get; set; }

    [JsonPropertyName("reportingMTA")]
    public string? ReportingMTA { get; set; }

    [JsonPropertyName("remoteMtaIp")]
    public string? RemoteMtaIp { get; set; }
}