using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class Message
{
    public Message() { }

    public Message(Role role, params ContentBlock[] content)
    {
        Role = role;
        Content = content;
    }

    [JsonPropertyName("content")]
    public IReadOnlyList<ContentBlock>? Content { get; set; }

    [JsonPropertyName("role")]
    public Role? Role { get; set; }
}