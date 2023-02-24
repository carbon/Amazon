#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Kms.Exceptions;

public sealed class KmsError
{
    [JsonPropertyName("__type")]
    public string Type { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; }
}