using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class Message
{
    [JsonPropertyName("content")]
    public List<ContentBlock>? Content { get; set; }

    [JsonPropertyName("role")]
    public Role? Role { get; set; }
}