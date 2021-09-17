#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Kms.Exceptions;

public sealed class KmsError
{
    [JsonPropertyName("__type")]
    public string Type { get; init; }

    [JsonPropertyName("Message")]
    public string Message { get; init; }
}