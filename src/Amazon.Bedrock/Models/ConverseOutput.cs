#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class ConverseOutput
{
    [JsonPropertyName("message")]
    public Message Message { get; init; }
}