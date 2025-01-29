#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Bedrock;

public sealed class Error
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}