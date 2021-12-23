#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SesMail
{
    [JsonPropertyName("source")]
    public string Source { get; init; }

    [JsonPropertyName("destination")]
    public string[] Destination { get; init; }

    [JsonPropertyName("messageId")]
    public string MessageId { get; init; }

#nullable enable

    [JsonPropertyName("headersTruncated")]
    public bool? HeadersTruncated { get; init; }

    [JsonPropertyName("headers")]
    public SesHeader[]? Headers { get; init; }

    [JsonPropertyName("commonHeaders")]
    public SesCommonHeaders? CommonHeaders { get; init; }
}
