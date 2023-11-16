#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ErrorResult
{
    [JsonPropertyName("__type")]
    public string Type { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; }
}